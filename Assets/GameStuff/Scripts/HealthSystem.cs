using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthSystem
{
    public PlayerStats playerStats;
    public TextMeshProUGUI[] healthTexts;
    private Player player;
    
    public float maxHealth;
    public float currentHealth;

    public HealthSystem(PlayerStats playerStats, Player player)
    {
        this.playerStats = playerStats;
        this.playerStats.OnStatChange += OnStatChange;
        maxHealth = playerStats.GetAttributeValue(Attribute.MaxHealth);
        currentHealth = maxHealth;
        healthTexts = new TextMeshProUGUI[] { playerStats.gameObject.GetComponent<Player>().healthText };
        this.player = player;
    }
    public void InstantHeal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthTexts();
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthTexts();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void UpdateHealthTexts()
    {
        for (int i = 0; i < healthTexts.Length; i++)
        {
            if (healthTexts[i] != null)
            {
                healthTexts[i].text = currentHealth.ToString();
            }
        }
    }
    public void Die()
    {
        player.Die();
    }

    public void OnStatChange()
    {
        maxHealth = playerStats.GetAttributeValue(Attribute.MaxHealth);
    }
}
