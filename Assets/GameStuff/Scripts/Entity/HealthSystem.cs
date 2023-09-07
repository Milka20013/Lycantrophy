using TMPro;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class HealthSystem : MonoBehaviour
{
    public TextMeshProUGUI[] healthTexts;
    protected Stats stats;
    [HideInInspector] public float maxHealth;
    [SerializeField] private Attribute maxHealthAttribute;
    protected float currentHealth;

    public bool isDead { get; private set; }

    public delegate void DeathHandler(GameObject killer);
    public DeathHandler onDeath;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    protected virtual void Start()
    {
        stats.OnStatChange += OnStatChange;
        stats.GetAttributeValue(maxHealthAttribute, out maxHealth);
        currentHealth = maxHealth;
        UpdateHealthTexts();
    }
    public void InstantHeal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthTexts();
    }
    public virtual void TakeDamage(float amount, GameObject attacker)
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

    public virtual void OnStatChange()
    {
        stats.GetAttributeValue(maxHealthAttribute, out maxHealth);
        UpdateHealthTexts();
    }
}
