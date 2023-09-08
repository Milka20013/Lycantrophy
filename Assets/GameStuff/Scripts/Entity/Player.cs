using StarterAssets;
using System;
using TMPro;
using UnityEngine;

[Serializable]
public record PlayerData : SaveProfileData
{
    public PlayerData(Vector3 position)
    {
        this.position = position;
    }
    public Vector3 position;
}

public class Player : MonoBehaviour
{
    public PlayerInventory playerInventory { get; private set; }
    public Stats playerStats { get; private set; }
    public TextMeshProUGUI healthText;
    public GameObject deathCanvas;
    public BoxCollider playerHitbox;

    public HealthSystem healthSystem { get; private set; }
    public TakeDamage takeDamage;
    [SerializeField] Attribute maxHealthAttribute;
    [SerializeField] private Amplifier[] levelUpAmplifiers;

    private ThirdPersonController moveController;



    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        healthSystem = GetComponent<HealthSystem>();
        playerStats = GetComponent<Stats>();
        moveController = GetComponent<ThirdPersonController>();
        healthSystem.onDeath += Die;
        GetComponent<Levelling>().OnLevelUp += LevelUp;
    }

    public void Die(GameObject killer)
    {
        deathCanvas.SetActive(true);
        moveController.enabled = false;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 80);
    }

    public void Respawn()
    {
        playerStats.GetAttributeValue(maxHealthAttribute, out float healthToHeal);
        healthSystem.Respawn(healthToHeal * 0.25f);
        takeDamage.isDead = false;
        moveController.enabled = true;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        deathCanvas.SetActive(false);
    }

    public void LevelUp(int currentLevel)
    {
        for (int i = 0; i < levelUpAmplifiers.Length; i++)
        {
            levelUpAmplifiers[i].value = currentLevel - 1;
        }
        playerStats.RegisterAmplifiers(levelUpAmplifiers);
    }
}
