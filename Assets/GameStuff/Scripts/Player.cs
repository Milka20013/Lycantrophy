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
    public Attacker playerAttack;
    public Stats playerStats;
    public Levelling levelling;
    public StatMenu statMenu;
    public TextMeshProUGUI healthText;

    [HideInInspector] public HealthSystem healthSystem;

    [SerializeField] private ThirdPersonController moveController;

    [HideInInspector] public bool isDead = false;
    public float respawnCooldown = 10f;
    [HideInInspector] public float respawnTimer = 10f;
    private void Update()
    {
        if (isDead && respawnTimer > 0f)
        {
            respawnTimer -= Time.deltaTime;
        }
    }
    public void TakeDamage(float amount, GameObject attacker)
    {
        healthSystem.TakeDamage(amount, attacker);
    }
    public void AddItem(ItemBlueprint item, int quantity = 1)
    {
        playerInventory.AddItem(item, quantity);
    }
    public void AddExp(float amount)
    {
        levelling.AddExp(amount);
    }

    public void Die()
    {
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
    }

    void ISaveable.Save(ref GameData data)
    {
        data.playerData = new GameData.PlayerData(transform.position);
    }

    void ISaveable.Load(GameData data)
    {
        transform.position = data.playerData.position;
    }
}
