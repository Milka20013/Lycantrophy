using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(Detector))]
public class AggressiveMob : Mob
{
    public Attacker attacker;
    public Detector detector;

    private void Start()
    {
        agent.stoppingDistance = attacker.attackRange - 0.1f;
        detector.onDetection += HandleDetection;
    }
    public void HandleDetection(bool hasTarget)
    {
        if (hasTarget)
        {
            SetDestination(attacker.attackRange - 0.1f, detector.targets[0].transform.position);
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

    
}
