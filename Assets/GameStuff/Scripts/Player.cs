using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

//this class has all the references to the other classes attached to the player
//and the methods with the player
//plus other things on the player

public class Player : MonoBehaviour, ISaveable
{
    public PlayerInventory playerInventory;
    public EquipmentInventory equipmentInventory;
    public Stats playerStats;
    public StatMenu statMenu;
    public TextMeshProUGUI healthText;
    public GameObject deathCanvas;

    public HealthSystem healthSystem;

    [SerializeField] private ThirdPersonController moveController;

    public bool isDead {  get; private set; }
    public float respawnCooldown = 10f;
    [HideInInspector] public float respawnTimer = 10f;
    private void Awake()
    {
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
        transform.rotation = Quaternion.Euler(0,transform.rotation.y, 80);
    }

    public void Respawn()
    {
        healthSystem.InstantHeal(playerStats.GetAttributeValue(Attribute.MaxHealth) * 0.25f);
        moveController.enabled = true;
        isDead = false;
        respawnTimer = respawnCooldown;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        deathCanvas.SetActive(false);
    }

    public void Save(ref GameData data)
    {
        data.playerData = new GameData.PlayerData(transform.position);
    }

    public void Load(GameData data)
    {
        transform.position = data.playerData.position;
    }
}
