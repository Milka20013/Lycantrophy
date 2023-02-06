using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : Inventory
{
    public List<ItemStack> equippedItems;

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

}
