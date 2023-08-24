using UnityEngine;

public class NeutralMob : Mob
{
    public Attacker attackerScr;
    public Detector detector;

    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = attackerScr.attackRange - 0.1f;
        detector.onDetection += HandleDetection;
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        base.OnHit(amount, attacker);
        HandleHit(attacker);
    }
    public void HandleDetection(bool hasTarget)
    {
        if (hasTarget)
        {
            occupied = true;
            SetDestination(detector.targets[0].transform.position, attackerScr.attackRange - 0.1f);
            if (detector.TryDetectTargets(out GameObject[] targets, attackerScr.attackRange))
            {
                attackerScr.canAttack = true;
            }
            else
            {
                attackerScr.canAttack = false;
            }
        }
        else
        {
            CalmDown();
        }

    }
    public void HandleHit(GameObject attacker)
    {
        if (attacker != null)
        {
            detector.autoDetect = true;
            occupied = true;
        }
        else
        {
            CalmDown();
        }

    }

    public override void CalmDown()
    {
        base.CalmDown();
        detector.autoDetect = false;
        attackerScr.canAttack = false;
    }
}
