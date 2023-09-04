using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthSystem))]
[RequireComponent(typeof(DropTable))]
[RequireComponent(typeof(TakeDamage))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(HitboxResizer))]

public class Mob : MonoBehaviour, IProvokable
{
    protected NavMeshAgent agent;
    protected HealthSystem healthSystem;
    protected Stats stats;
    protected TakeDamage takeDamage;
    protected DropTable dropTable;
    [SerializeField] protected Attribute moveSpeedAttribute;
    protected EntityAnimator animator;
    protected bool hasAnimator;

    [Tooltip("How far can the mob wander from its start position")]
    public float wanderDistance = 5;
    [Tooltip("Interval between wander target getting in seconds")]
    public float wanderCooldown = 5;

    [HideInInspector] public bool occupied;

    private Vector3 startPosition;

    public bool isDead { get; private set; }


    public delegate void DeathHandler(GameObject killer);
    public event DeathHandler OnDeath;

    protected double previousWanderTime;
    protected virtual void Update()
    {
        if (!occupied && Time.timeAsDouble - previousWanderTime > wanderCooldown)
        {
            previousWanderTime = Time.timeAsDouble + Random.Range(0, wanderCooldown);
            Wander();
        }
    }
    protected virtual void Awake()
    {
        FindReferences();
        previousWanderTime = Random.Range(0, wanderCooldown);
        startPosition = transform.position;
        healthSystem.onDeath += Die;
        takeDamage.OnHit += OnHit;
    }

    protected virtual void FindReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        healthSystem = GetComponent<HealthSystem>();
        stats = GetComponent<Stats>();
        takeDamage = GetComponent<TakeDamage>();
        dropTable = GetComponent<DropTable>();
        if (TryGetComponent(out animator))
        {
            hasAnimator = true;
        }
    }

    protected virtual void Start()
    {
        agent.speed = stats.GetAttributeValue(moveSpeedAttribute);
        //makes the agent move at an almost constant speed
        agent.acceleration = 1000;
    }

    /*public void RegisterMobData(MobData mobdata)
    {
        stats.CreateAmplifierSystem(mobdata);
        dropTable.mobData = mobdata;
    }*/

    public virtual void Provoke(Collider provoker, float occupationTime)
    {
        previousWanderTime = Time.timeAsDouble + Random.Range(occupationTime, wanderCooldown);

        SetDestination(provoker.transform.position);
    }


    public void SetDestination(Vector3 position, float stoppingDistance = 0.1f)
    {
        if (isDead)
        {
            return;
        }
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(position);
        if (hasAnimator)
        {
            //it isn't the perfect timing but good enough
            float timeToStop;
            if (agent.pathPending)
            {
                timeToStop = Vector3.Distance(position, transform.position) / agent.speed + 0.2f;
            }
            else
            {
                timeToStop = (agent.remainingDistance - stoppingDistance) / agent.speed + 0.2f;
            }
            animator.ChangeAnimationStateThenIdle(animator.walk, timeToStop);
        }
    }

    /// <summary>
    /// This method projects the position to the NavMesh. 
    /// If it couldn't be projected, then the normal position is used
    /// </summary>
    public void SetDestinationProjected(Vector3 position, float stoppingDistance = 0.1f)
    {
        if (NavMesh.SamplePosition(position, out var hit, wanderDistance, 1))
        {
            SetDestination(hit.position, stoppingDistance);
        }
        else
        {
            SetDestination(position, stoppingDistance);
        }
    }

    public void ReturnToStartPosition()
    {
        agent.stoppingDistance = 0f;
        SetDestination(startPosition);
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
        SetDestination(finalPosition);
    }



    public void Die(GameObject killer)
    {
        isDead = true;
        OnDeath?.Invoke(killer);
        takeDamage.Die(killer);
        Destroy(gameObject);
    }


}
