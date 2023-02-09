using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthSystem))]
public class Mob : MonoBehaviour
{
    public NavMeshAgent agent;
    public HealthSystem healthSystem;
    public Stats stats;
    public Attacker attacker;
    public Detector detector;

    public Vector3 startPosition;


    public delegate void DeathHandler(GameObject killer);
    public event DeathHandler OnDeath;
    private void Start()
    {
        startPosition = transform.position;
        agent.stoppingDistance = attacker.attackRange - 0.1f;
        detector.onDetection += HandleDetection;
        healthSystem.onDeath += Die;
    }
    public void HandleDetection(bool hasTarget)
    {
        if (hasTarget)
        {
            SetDestination();
            if (detector.TryDetectTargets(out GameObject[] targets, attacker.attackRange))
            {
                attacker.canAttack = true;
            }
            else
            {
                attacker.canAttack = false;
            }
        }
        else
        {
            ReturnToStartPosition();
        }
        
    }

    public void SetDestination()
    {
        agent.stoppingDistance = attacker.attackRange - 0.1f;
        agent.SetDestination(detector.targets[0].transform.position);
    }

    public void ReturnToStartPosition()
    {
        agent.stoppingDistance = 0f;
        agent.SetDestination(startPosition);
    }

    public void Die(GameObject killer)
    {
        OnDeath?.Invoke(killer);
        Destroy(gameObject);
    }
}
