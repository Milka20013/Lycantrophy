using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour, ISaveable
{
    public GameObject inventoryUI;
    public ItemSpawner itemSpawner;
    public ItemManager itemManager;

    public List<ItemStack> stacksInInventory = new List<ItemStack>();

    public EquipmentInventory equipmentInventory;

    private void Awake()
    {
        if (equipmentInventory == null)
        {
            equipmentInventory = new EquipmentInventory(itemManager);
        }
    }
    public void AddItem(Item item, int quantity = 1, bool spawn = true)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            quantity -= stacksInInventory[i].FillStack(item, quantity);
        }
        if (quantity > 0)
        {
            int freeSpace = itemSpawner.slots.Length - stacksInInventory.Count;
            ItemStack[] createdStacks = ItemStack.CreateItemStacks(item, quantity);
            for (int i = 0; i < createdStacks.Length; i++)
            {
                createdStacks[i].playerInventory = this;
            }
            if (freeSpace >= createdStacks.Length)
            {
                stacksInInventory.AddRange(createdStacks);
            }
            else
            {
                for (int i = 0; i < freeSpace; i++)
                {
                    stacksInInventory.Add(createdStacks[i]);
                }
                //here should go the logic when items couldn't fit in the inv
            }

        }
        if (itemSpawner.slots[0].isActiveAndEnabled && spawn)
        {
            SpawnItems();
        }
    }
    public void AddItem(ItemBlueprint item, int quantity = 1)
    {
        AddItem(itemManager.GetItem(item), quantity);
    }
    public void OnOpenInventory(InputValue value)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        Cursor.lockState = inventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        SpawnItems();
    }
    private void SpawnItems()
    {
        if (itemSpawner.player == null)
        {
            itemSpawner.player = gameObject.GetComponent<Player>();
        }
        itemSpawner.itemStacks = stacksInInventory;
        itemSpawner.SpawnItems();
    }


    public void RemoveDeadItems()
    {
        stacksInInventory.RemoveAll(x => x.state == ItemStack.StackState.Dead);
        equipmentInventory.RemoveDeadItems();
        itemSpawner.RemoveDeadItems();
    }

    public void RemoveItem(ItemStack itemStack)
    {
        itemStack.state = ItemStack.StackState.Dead;
        RemoveDeadItems();
    }
    public void UnequipItem(ItemStack itemStack)
    {
        equipmentInventory.UnequipItem(itemStack);
        stacksInInventory.Add(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }
    public void EquipItem(ItemStack itemStack)
    {
        equipmentInventory.EquipItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }

    public bool ItemIsEquippedByName(ItemStack itemStack)
    {
        return equipmentInventory.ItemIsEquippedByName(itemStack);
    }
    public bool ItemIsEquippedByRef(ItemStack itemStack)
    {
        return equipmentInventory.ItemIsEquippedByRef(itemStack);
    }

    public void Save(ref GameData data)
    {
        data.inventoryData.inventoryItems = GameData.InventoryData.CreateItemDatas(stacksInInventory);
        equipmentInventory.Save(ref data);
    }

    public void Load(GameData data)
    {
        equipmentInventory = new EquipmentInventory(itemManager);
        for (int i = 0; i < data.inventoryData.inventoryItems.Length; i++)
        {
            AddItem(itemManager.GetItem(data.inventoryData.inventoryItems[i].itemId), data.inventoryData.inventoryItems[i].quantity, false);
        }
        equipmentInventory.Load(data);
        InstantiateItems(data);
    }

    private void InstantiateItems(GameData data)
    {
        itemSpawner.player = gameObject.GetComponent<Player>();
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            itemSpawner.InstanstiateItem(stacksInInventory[i], data.inventoryData.inventoryItems[i].slotId);
        }
    }
}
