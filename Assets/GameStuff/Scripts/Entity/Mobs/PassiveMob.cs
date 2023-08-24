using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveMob : Mob
{
    [SerializeField] private float fleeDistance;
    [Tooltip("When should the mob wander back after a hit")]
    [SerializeField] private float wanderBackCooldown;

    protected override void Update()
    {
        base.Update();
        if (occupied && Time.timeAsDouble - takeDamage.previousHitTime > wanderBackCooldown)
        {
            CalmDown();
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        base.OnHit(amount, attacker);
        Flee(attacker, 0, 0);
    }

    public void Flee(GameObject attacker, float angle , int tries)
    {
        Vector3 direction = transform.position - attacker.transform.position;
        direction.y = 0;
        Vector3 point = transform.position + direction * Random.Range(fleeDistance / 2, fleeDistance);
        point = Quaternion.AngleAxis(angle, Vector3.up) * point;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, agent.height * 2, NavMesh.AllAreas))
        {
            SetDestination(hit.position, 0);
        }
        else if (tries <= 12)
        {
            Flee(attacker, angle + Random.Range(15,45), tries + 1);
        }
        
    }
}
