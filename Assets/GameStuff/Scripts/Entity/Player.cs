using StarterAssets;
using System;
using TMPro;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public PlayerData(Vector3 position)
    {
        this.position = position;
    }
    public Vector3 position;
}

public class Player : MonoBehaviour, ISaveable
{
    public PlayerInventory playerInventory { get; private set; }
    public Stats playerStats { get; private set; }
    public TextMeshProUGUI healthText;
    public GameObject deathCanvas;
    public BoxCollider playerHitbox;

    public HealthSystem healthSystem { get; private set; }
    [SerializeField] Attribute maxHealthAttribute;

    private ThirdPersonController moveController;

    public bool isDead { get; private set; }
    public float respawnCooldown = 10f;
    [HideInInspector] public float respawnTimer = 10f;
    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        healthSystem = GetComponent<HealthSystem>();
        playerStats = GetComponent<Stats>();
        moveController = GetComponent<ThirdPersonController>();



        healthSystem.onDeath += Die;
    }
    private void Update()
    {
        if (isDead && respawnTimer > 0f)
        {
            respawnTimer -= Time.deltaTime;
        }
    }

    public void Die(GameObject killer)
    {
        deathCanvas.SetActive(true);
        moveController.enabled = false;
        isDead = true;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 80);
    }

    public void Respawn()
    {
        healthSystem.InstantHeal(playerStats.GetAttributeValue(maxHealthAttribute) * 0.25f);
        moveController.enabled = true;
        isDead = false;
        respawnTimer = respawnCooldown;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        deathCanvas.SetActive(false);
    }

    public void Save(ref GameData data)
    {
        data.playerData = new PlayerData(transform.position);
    }

    public void Load(GameData data)
    {
        transform.position = data.playerData.position;
    }
}
