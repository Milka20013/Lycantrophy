using UnityEngine;

public class NeutralMob : AggressiveMob
{
    public override void OnHit(float amount, GameObject attacker)
    {
        base.OnHit(amount, attacker);
        HandleHit(attacker);
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
        mobAttack.canAttack = false;
    }
}
