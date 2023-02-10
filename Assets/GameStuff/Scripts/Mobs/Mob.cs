using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
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


    public delegate void DeathHandler(GameObject killer);
    public event DeathHandler OnDeath;

    private double previousTime;
    private void Update()
    {
        if (!occupied && Time.timeAsDouble - previousTime > wanderCooldown)
        {
            previousTime = Time.timeAsDouble;
            Wander();
        }
    }
    private void Awake()
    {
        startPosition = transform.position;
        healthSystem.onDeath += Die;
    }

    public void RegisterMobData(MobData mobdata)
    {
        stats.CreateAmplifierSystem(mobdata);
        dropTable.mobData = mobdata;
    }

    public void SetDestination(float stoppingDistance, Vector3 position)
    {
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(position);
    }

    public void ReturnToStartPosition()
    {
        agent.stoppingDistance = 0f;
        agent.SetDestination(startPosition);
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
        OnDeath?.Invoke(killer);
        takeDamage.Die();
    }
}
