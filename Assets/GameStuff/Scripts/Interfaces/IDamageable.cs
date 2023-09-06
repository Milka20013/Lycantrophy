using UnityEngine;

public interface IDamageable
{

    void RegisterDamage(float amount, GameObject attacker, Stats attackerStats);
    bool IsDead();
}
