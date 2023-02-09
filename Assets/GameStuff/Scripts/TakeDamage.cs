using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class TakeDamage : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    public void RegisterDamage(float amount, GameObject attacker)
    {
        healthSystem.TakeDamage(amount, attacker);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
