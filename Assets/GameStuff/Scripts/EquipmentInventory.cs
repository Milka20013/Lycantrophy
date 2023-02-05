using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : ISaveable
{
    public ItemManager itemManager;
    public List<ItemStack> equippedItems;

    public EquipmentInventory(ItemManager itemManager)
    {
        equippedItems = new List<ItemStack>();
        this.itemManager = itemManager;
    }

    private void AddItem(GameData.InventoryData.ItemData data)
    {
        ItemStack[] items = ItemStack.CreateItemStacks(itemManager.GetItem(data.itemId),data.quantity);
        equippedItems.AddRange(items);
    }
    public void UnequipItem(ItemStack itemStack)
    {
        equippedItems.Remove(itemStack);
    }
    public void EquipItem(ItemStack itemStack)
    {
        equippedItems.Add(itemStack);
    }

    public bool ItemIsEquippedByName(ItemStack itemStack)
    {
        for (int i = 0; i < equippedItems.Count; i++)
        {
            if (itemStack.item.itemName == equippedItems[i].item.itemName)
            {
                return true;
            }
        }
        return false;
    }
    public bool ItemIsEquippedByRef(ItemStack itemStack)
    {
        return equippedItems.Contains(itemStack);
    }

    public void RemoveDeadItems()
    {
        equippedItems.RemoveAll(x => x.state == ItemStack.StackState.Dead);
    }

    //these methods won't be called by the manager, because it is not a monobehaviour
    //called in the inventory instead
    public void Save(ref GameData data)
    {
        data.inventoryData.equipmentItems = GameData.InventoryData.CreateItemDatas(equippedItems);
    }

    public void Load(GameData data)
    {
        for (int i = 0; i < data.inventoryData.equipmentItems.Length; i++)
        {
            AddItem(data.inventoryData.equipmentItems[i]);
        }
    }
}
