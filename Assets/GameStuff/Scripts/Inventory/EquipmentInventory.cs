using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentInventory : Inventory
{

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void UnequipItem(ItemStack itemStack)
    {
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
        player.playerStats.UnRegisterAmplifiers(itemStack.itemUI.GetComponent<Equipment>().equipmentItem.amplifiers);
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            var amps = stacksInInventory[i].itemUI.GetComponent<Equipment>().equipmentItem.amplifiers;
            player.playerStats.RegisterAmplifiers(amps);
        }
    }

    public void EquipItem(ItemStack itemStack)
    {
        player.playerStats.RegisterAmplifiers(itemStack.itemUI.GetComponent<Equipment>().equipmentItem.amplifiers);
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
