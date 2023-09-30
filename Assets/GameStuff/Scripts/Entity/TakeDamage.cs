using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IDamageable, IDetectable
{
    [Header("Optional")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Stats stats;

    [SerializeField] private CombatCondition[] conditionsArray;
    private HashSet<CombatCondition> conditions;
    private CombatSystem combatSystem;

    public delegate void HitHandler(float amount, GameObject attacker);
    public HitHandler onHit;

    public bool isDead { get; set; }
    public double previousHitTime { get; set; }

    private void Awake()
    {
        if (healthSystem == null)
        {
            healthSystem = GetComponent<HealthSystem>();
        }
        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }
        conditions = conditionsArray.ToHashSet();
        combatSystem = new CombatSystem(stats, conditions);
    }

    private void Start()
    {
        healthSystem.onDeath += Die;
    }

    public float RegisterDamage(float amount, GameObject attacker, Stats attackerStats)
    {
        float damage = combatSystem.CalculateDamage(attackerStats, amount);
        healthSystem.TakeDamage(damage, attacker);
        onHit?.Invoke(damage, attacker);
        return damage;
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
