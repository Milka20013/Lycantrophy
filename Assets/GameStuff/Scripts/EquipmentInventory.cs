using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentInventory : Inventory
{
    public PlayerInventory playerInventory;

    public override void OnOpenInventory(InputValue value)
    {
        base.SpawnItems();
    }
    public void UnequipItem(ItemStack itemStack)
    {
        stacksInInventory.Remove(itemStack);
        playerInventory.AddItem(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }

    public bool ItemIsEquippedByName(ItemStack itemStack)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            if (itemStack.item.itemName == stacksInInventory[i].item.itemName)
            {
                return true;
            }
        }
        return false;
    }
    public bool ItemIsEquippedByRef(ItemStack itemStack)
    {
        return stacksInInventory.Contains(itemStack);
    }


}
