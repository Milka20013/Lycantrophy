using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Stats))]
public class HealthSystem : MonoBehaviour
{
    public TextMeshProUGUI[] healthTexts;
    public Stats stats;
    public float maxHealth { get; set; }
    private float currentHealth;

    public delegate void DeathHandler(GameObject killer);
    public DeathHandler onDeath;

    private void Start()
    {
        stats.OnStatChange += OnStatChange;
        maxHealth = stats.GetAttributeValue(Attribute.MaxHealth);
        currentHealth = maxHealth;
    }
    public void InstantHeal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthTexts();
    }
    public void TakeDamage(float amount, GameObject attacker)
    {
        currentHealth -= amount;
        UpdateHealthTexts();
        if (currentHealth <= 0)
        {
            onDeath?.Invoke(attacker);
        }
    }
    public void UpdateHealthTexts()
    {
        if (healthTexts.Length == 0)
        {
            return;
        }
        for (int i = 0; i < healthTexts.Length; i++)
        {
            if (healthTexts[i] != null)
            {
                healthTexts[i].text = currentHealth.ToString();
            }
        }
    }

    public void OnStatChange()
    {
        maxHealth = stats.GetAttributeValue(Attribute.MaxHealth);
    }
}
