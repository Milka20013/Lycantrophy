using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class TakeDamage : MonoBehaviour, IDamageable
{
    private HealthSystem healthSystem;

    public delegate void HitHandler(float amount, GameObject attacker);
    public HitHandler OnHit;

    public bool isDead { get; set; }
    public double previousHitTime { get; set; }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.onDeath += Die;
    }

    public void RegisterDamage(float amount, GameObject attacker)
    {
        healthSystem.TakeDamage(amount, attacker);
        OnHit?.Invoke(amount, attacker);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Die(GameObject killer)
    {
        isDead = true;
    }
}
