using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, ISaveable
{

    public string id;

    public enum InventoryTag
    {
        None, Player, Equipment
    }
    public InventoryTag inventoryTag;

    public GameObject inventoryUI;
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

    public virtual bool AddItemStack(ItemStack itemStack)
    {
        stacksInInventory.Add(itemStack);
        return true;
    }
    public void DeleteItem(ItemStack itemStack)
    {
        itemStack.state = ItemStack.StackState.Dead;
        DeleteDeadItems();
    }
    public void DeleteDeadItems()
    {
        stacksInInventory.RemoveAll(x => x.state == ItemStack.StackState.Dead);
        itemSpawner.DeleteDeadItems();
    }
    private void RemoveItem(ItemStack item)
    {
        stacksInInventory.Remove(item);
    }
    public virtual void OnOpenInventory(InputValue value)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        Cursor.lockState = inventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        SpawnItems();
    }
    protected void SpawnItems()
    {
        itemSpawner.itemStacks = stacksInInventory;
        itemSpawner.SpawnItems();
    }



    public void ChangeInventories(ItemStack item, Inventory targetInventory)
    {
        if (targetInventory.AddItemStack(item))
        {
            RemoveItem(item);
        }
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

    private void InstantiateItems(GameData.InventoryData.ItemData[] inventoryItems)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            itemSpawner.InstanstiateItem(stacksInInventory[i], inventoryItems[i].slotId);
        }
    }

}
