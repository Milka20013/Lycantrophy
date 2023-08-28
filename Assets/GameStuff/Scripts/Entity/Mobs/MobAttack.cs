using UnityEngine;

public class MobAttack : Attacker
{
    protected override void Update()
    {
        base.Update();
        if (canAttack && attackDelay <= 0f)
        {
            TryAttack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackOrigin.position, attackRange);
    }
}
