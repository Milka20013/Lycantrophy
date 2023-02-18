using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : Inventory
{
    public EquipmentInventory equipmentInventory;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(stacksInInventory.Count);
        }
    }
    public void EquipItem(ItemStack itemStack)
    {
        equipmentInventory.EquipItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }

}
