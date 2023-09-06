using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Stats stats;

    [SerializeField] private CombatCondition[] conditionsArray;
    private HashSet<CombatCondition> conditions;
    private CombatSystem combatSystem;

    public delegate void HitHandler(float amount, GameObject attacker);
    public HitHandler OnHit;

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
        ConvertConditionArrayToHashSet();
        combatSystem = new CombatSystem(stats, conditions);
    }

    private void ConvertConditionArrayToHashSet()
    {
        for (int i = 0; i < conditionsArray.Length; i++)
        {
            conditions.Add(conditionsArray[i]);
        }
    }

    private void Start()
    {
        healthSystem.onDeath += Die;
    }

    public void RegisterDamage(float amount, GameObject attacker, Stats attackerStats)
    {
        Debug.Log(amount);
        float damage = combatSystem.CalculateDamage(attackerStats, amount);
        Debug.Log(damage);
        healthSystem.TakeDamage(damage, attacker);
        OnHit?.Invoke(damage, attacker);
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
