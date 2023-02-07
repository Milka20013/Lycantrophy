using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : Inventory
{
    public EquipmentInventory equipmentInventory;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void EquipItem(ItemStack itemStack)
    {
        equipmentInventory.EquipItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;

    }


}
