using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : Inventory
{
    public OrbInventory orbInventory;

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
        orbInventory.EquipItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
    }

}
