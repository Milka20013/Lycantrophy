using TMPro;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class HealthSystem : MonoBehaviour
{
    public TextMeshProUGUI[] healthTexts;
    private Stats stats;
    public float maxHealth { get; set; }
    [SerializeField] private Attribute maxHealthAttribute;
    private float currentHealth;

    public bool isDead { get; private set; }

    public delegate void DeathHandler(GameObject killer);
    public DeathHandler onDeath;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void Start()
    {
        stats.OnStatChange += OnStatChange;
        maxHealth = stats.GetAttributeValue(maxHealthAttribute);
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
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        UpdateHealthTexts();
        if (currentHealth <= 0)
        {
            isDead = true;
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
        maxHealth = stats.GetAttributeValue(maxHealthAttribute);
    }
}
