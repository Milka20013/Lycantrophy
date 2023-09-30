using UnityEngine;

public interface IDamageable
{

    float RegisterDamage(float amount, GameObject attacker, Stats attackerStats);
    bool IsDead();
}
