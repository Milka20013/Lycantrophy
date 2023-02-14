using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class TakeDamage : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    public delegate void HitHandler(float amount,GameObject attacker);
    public HitHandler OnHit;

    public bool isDead { get; set; }
    public double previousHitTime { get; set; }

    private void Start()
    {
        healthSystem.onDeath += Die;
    }

    public void RegisterDamage(float amount, GameObject attacker)
    {
        healthSystem.TakeDamage(amount, attacker);
        OnHit?.Invoke(amount, attacker);
    }

    public void Die(GameObject killer)
    {
        isDead = true;
    }
}
