using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, ISaveable
{
    private string id;

    [ContextMenu("Generate unique id for the object")]
    private void GenerateGUI()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public GameObject inventoryUI;
    public ItemSpawner itemSpawner;
    public ItemManager itemManager;

    public List<ItemStack> stacksInInventory = new List<ItemStack>();

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
                createdStacks[i].inventory = this;
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
    public void RemoveItem(ItemStack itemStack)
    {
        itemStack.state = ItemStack.StackState.Dead;
        RemoveDeadItems();
    }
    public void OnOpenInventory(InputValue value)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        Cursor.lockState = inventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        SpawnItems();
    }
    private void SpawnItems()
    {
        if (itemSpawner.inventory == null)
        {
            itemSpawner.inventory = this;
        }
        itemSpawner.itemStacks = stacksInInventory;
        itemSpawner.SpawnItems();
    }

    public void RemoveDeadItems()
    {
        stacksInInventory.RemoveAll(x => x.state == ItemStack.StackState.Dead);
        itemSpawner.RemoveDeadItems();
    }
    public void Save(ref GameData data)
    {
        int index = data.GetIndex(id);
        var invData = new GameData.InventoryData(GameData.InventoryData.CreateItemDatas(stacksInInventory), id);
        if (index == -1)
        {
            data.inventoryDatas.Add(invData);
        }
        else
        {
            data.inventoryDatas[index] = invData;
        }
    }

    public void Load(GameData data)
    {
        int index = data.GetIndex(id);
        var invItems = data.inventoryDatas[index].inventoryItems;
        for (int i = 0; i < data.inventoryDatas[index].inventoryItems.Length; i++)
        {
            AddItem(itemManager.GetItem(invItems[i].itemId), invItems[i].quantity, false);
        }
        InstantiateItems(invItems);
    }

    private void InstantiateItems(GameData.InventoryData.ItemData[] inventoryItems)
    {
        itemSpawner.inventory = this;
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            itemSpawner.InstanstiateItem(stacksInInventory[i], inventoryItems[i].slotId);
        }
    }
}

