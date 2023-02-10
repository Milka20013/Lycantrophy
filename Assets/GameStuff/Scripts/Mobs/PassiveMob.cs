using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveMob : Mob
{
    [SerializeField] private float fleeDistance;
    [Tooltip("When should the mob wander back after a hit")]
    [SerializeField] private float wanderBackCooldown;

    private double previousHitTime;
    private void Start()
    {
        takeDamage.OnHit += OnHit;
    }

    private void Update()
    {
        if (occupied && Time.timeAsDouble - previousHitTime > wanderBackCooldown)
        {
            occupied = false;
        }
    }

    public void OnHit(float amount, GameObject attacker)
    {
        if (healthSystem.isDead)
        {
            return;
        }
        previousHitTime = Time.timeAsDouble;
        occupied = true;
        Flee(attacker, 0 , 0);
    }

    public void Flee(GameObject attacker, float angle , int tries)
    {
        Vector3 direction = transform.position + (transform.position - attacker.transform.position) * fleeDistance;
        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(direction, out hit, fleeDistance, 1))
        {
            agent.SetDestination(hit.position);
        }
        else if (tries <= 3)
        {
            Flee(attacker, angle + 30, tries + 1);
        }
        
    }
}
