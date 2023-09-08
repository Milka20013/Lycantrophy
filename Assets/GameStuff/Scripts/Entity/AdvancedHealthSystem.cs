using System.Collections;
using UnityEngine;

public class AdvancedHealthSystem : HealthSystem
{
    [SerializeField] Attribute healthRegenAttribute;
    [Tooltip("Regens every x seconds")]
    public float regenCooldown = 5;
    private float healthRegen;
    private bool isRegenerating;

    protected override void Start()
    {
        base.Start();
        stats.GetAttributeValue(healthRegenAttribute, out healthRegen);
        StartRegen();
    }
    public override void TakeDamage(float amount, GameObject attacker)
    {
        base.TakeDamage(amount, attacker);
        StartRegen();
    }

    public override void OnStatChange()
    {
        base.OnStatChange();
        stats.GetAttributeValue(healthRegenAttribute, out healthRegen);
        StartRegen();
    }

    public void StartRegen()
    {
        if (currentHealth > 0f && !isRegenerating)
        {
            StartCoroutine(nameof(RegenHealth));
        }
    }

    IEnumerator RegenHealth()
    {
        for (; ; )
        {
            isRegenerating = true;
            if (currentHealth >= maxHealth)
            {
                isRegenerating = false;
                break;
            }
            yield return new WaitForSeconds(regenCooldown);
            InstantHeal(healthRegen * maxHealth);
        }
    }
}
