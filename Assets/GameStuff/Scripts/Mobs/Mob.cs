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

    private Vector3 startPosition;


    public delegate void DeathHandler(GameObject killer);
    public event DeathHandler OnDeath;
    private void Awake()
    {
        startPosition = transform.position;
        healthSystem.onDeath += Die;
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

    public void Die(GameObject killer)
    {
        OnDeath?.Invoke(killer);
        takeDamage.Die();
    }
}
