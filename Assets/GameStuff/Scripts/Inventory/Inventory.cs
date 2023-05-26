using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct InventoryData
{
    public string id;
    public ItemData[] inventoryItems;

    public static ItemData[] CreateItemDatas(List<ItemStack> itemStacks)
    {
        ItemData[] itemDatas = new ItemData[itemStacks.Count];
        for (int i = 0; i < itemDatas.Length; i++)
        {
            itemDatas[i] = new ItemData(itemStacks[i].item.id, itemStacks[i].quantity, itemStacks[i].itemUI == null ? -1 : itemStacks[i].itemUI.slotId);
        }
        return itemDatas;
    }

    public InventoryData(ItemData[] inventoryItems, string id)
    {
        this.id = id;
        this.inventoryItems = inventoryItems;
    }
}

public class Inventory : MonoBehaviour, ISaveable
{

    public string id;

    public enum InventoryTag
    {
        None, Player, Equipment
    }
    public InventoryTag inventoryTag;

    [SerializeField] private GameObject inventoryUI;
    public ItemSpawner itemSpawner;
    public ItemManager itemManager;

    public Player player { get; set; }

    public List<ItemStack> stacksInInventory = new List<ItemStack>();
    public void AddItem(Item item, int quantity = 1)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            quantity -= stacksInInventory[i].FillStack(item, quantity);
        }
        if (quantity > 0)
        {
            int freeSpace = itemSpawner.slots.Length - stacksInInventory.Count;
            ItemStack[] createdStacks = ItemStack.CreateItemStacks(item, quantity);
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
        if (itemSpawner.slots[0].isActiveAndEnabled)
        {
            SpawnItems();
        }
    }
    public void AddItem(ItemBlueprint item, int quantity = 1)
    {
        AddItem(itemManager.GetItem(item), quantity);
    }

    

    //Deleting affects the actual gameObject not just the reference to the item
    public void DeleteItem(ItemStack itemStack)
    {
        itemStack.state = ItemStack.StackState.Dead;
        itemSpawner.DeleteDeadItems();

    }

    public void OnOpenInventory(InputValue value)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        Cursor.lockState = inventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        SpawnItems();
    }
    protected void SpawnItems()
    {
        itemSpawner.SpawnItems();
    }

    public void ChangeInventories(ItemStack item, Inventory targetInventory)
    {
        targetInventory.RegisterItemStack(item);
        UnRegisterItemStack(item);
    }

    public virtual void RegisterItemStack(ItemStack itemStack)
    {
        stacksInInventory.Add(itemStack);
    }
    public virtual void UnRegisterItemStack(ItemStack item)
    {
        stacksInInventory.Remove(item);
    }

    public void Save(ref GameData data)
    {
        int index = data.GetIndex(id);
        var invData = new InventoryData(InventoryData.CreateItemDatas(stacksInInventory), id);
        if (index == -1)
        {
            data.inventoryDatas.Add(invData);
        }
        else
        {
            data.inventoryDatas[index] = invData;
        }
    }

    public virtual void Load(GameData data)
    {
        int index = data.GetIndex(id);
        if (index == -1)
        {
            return;
        }
        var invItems = data.inventoryDatas[index].inventoryItems;
        for (int i = 0; i < data.inventoryDatas[index].inventoryItems.Length; i++)
        {
            AddItem(itemManager.GetItem(invItems[i].itemId), invItems[i].quantity);
        }
        InstantiateItems(invItems);
    }

    private void InstantiateItems(ItemData[] inventoryItems)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            itemSpawner.InstanstiateItem(stacksInInventory[i], inventoryItems[i].slotId);
        }
    }

}
