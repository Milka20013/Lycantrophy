using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : Inventory
{
    public Player player;
    public EquipmentInventory equipmentInventory;

    public void EquipItem(ItemStack itemStack)
    {
        equipmentInventory.AddItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }


}
