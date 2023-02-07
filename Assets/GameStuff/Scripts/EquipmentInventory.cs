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
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        Invoke("InitializeAmplifiers",0.05f);
        Invoke("InitializeAmplifiers", 0.1f);
    }

    private void InitializeAmplifiers()
    {
        base.OnOpenInventory(null);
    }
    public void UnequipItem(ItemStack itemStack)
    {
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }

    public void EquipItem(ItemStack itemStack)
    {
        
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
