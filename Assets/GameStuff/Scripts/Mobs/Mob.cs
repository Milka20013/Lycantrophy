using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(DropTable))]
[RequireComponent(typeof(TakeDamage))]

public class Mob : MonoBehaviour
{
    public NavMeshAgent agent;
    public HealthSystem healthSystem;
    public Stats stats;
    public TakeDamage takeDamage;
    public DropTable dropTable;

    [Tooltip("How far can the mob wander from its start position")]
    public float wanderDistance;
    [Tooltip("Interval between wander target getting")]
    public float wanderCooldown;

    [HideInInspector] public bool occupied;

    private Vector3 startPosition;

    public bool isDead { get; private set; }


    public delegate void DeathHandler(GameObject killer);
    public event DeathHandler OnDeath;

    private double previousTime;
    protected virtual void Update()
    {
        if (!occupied && Time.timeAsDouble - previousTime > wanderCooldown)
        {
            previousTime = Time.timeAsDouble + Random.Range(0, wanderCooldown);
            Wander();
        }
    }
    protected virtual void Awake()
    {
        previousTime = Random.Range(0, wanderCooldown);
        startPosition = transform.position;
        healthSystem.onDeath += Die;
        takeDamage.OnHit += OnHit;
    }

    public void RegisterMobData(MobData mobdata)
    {
        stats.CreateAmplifierSystem(mobdata);
        dropTable.mobData = mobdata;
    }


    public void SetDestination(Vector3 position, float stoppingDistance)
    {
        if (isDead)
        {
            return;
        }
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(position);
    }

    public void ReturnToStartPosition()
    {
        agent.stoppingDistance = 0f;
        agent.SetDestination(startPosition);
    }

    public virtual void OnHit(float amount, GameObject attacker)
    {
        if (healthSystem.isDead)
        {
            return;
        }
        takeDamage.previousHitTime = Time.timeAsDouble;
        occupied = true;
    }

    public virtual void CalmDown()
    {
        occupied = false;
        ReturnToStartPosition();
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderDistance;
        randomDirection += startPosition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderDistance, 1))
        {
            finalPosition = hit.position;
        }
        agent.SetDestination(finalPosition);
    }

    public void Die(GameObject killer)
    {
        isDead = true;
        OnDeath?.Invoke(killer);
        takeDamage.Die(killer);
        Destroy(gameObject);
    }
}
