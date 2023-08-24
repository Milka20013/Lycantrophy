using UnityEngine;

public interface IDamageable
{

    void RegisterDamage(float amount, GameObject killer);
    bool IsDead();
}
