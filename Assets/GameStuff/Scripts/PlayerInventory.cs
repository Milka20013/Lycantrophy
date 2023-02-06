using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : Inventory
{
    public EquipmentInventory equipmentInventory;
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
}
