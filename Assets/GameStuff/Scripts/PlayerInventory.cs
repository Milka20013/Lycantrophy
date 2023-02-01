using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public GameObject inventoryUI;
    public ItemSpawner ItemSpawner;
    public ItemManager ItemManager;

    public List<ItemStack> stacksInInventory = new List<ItemStack>();
    public List<ItemStack> stacksOnEquipmentPanel = new List<ItemStack>();
    public void AddItem(Item item, int quantity = 1)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            quantity -= stacksInInventory[i].FillStack(item, quantity);
        }
        if (quantity > 0)
        {
            int freeSpace = ItemSpawner.slots.Length - stacksInInventory.Count;
            ItemStack[] createdStacks = ItemStack.CreateItemStacks(item, quantity);
            for (int i = 0; i < createdStacks.Length; i++)
            {
                createdStacks[i].playerInventory = this;
            }
            if (freeSpace >= createdStacks.Length)
            {
                stacksInInventory.AddRange(createdStacks);
            }
            else
            {
                for (int i = 0; i < freeSpace; i++)
                {
                    stacksInInventory.Add(createdStacks[i]);
                }
                //here should go the logic when items couldn't fit in the inv
            }
            
        }
        if (ItemSpawner.slots[0].isActiveAndEnabled)
        {
            SpawnItems();
        }
    }
    public void AddItem(ItemBlueprint item, int quantity = 1)
    {
        AddItem(ItemManager.GetItem(item), quantity);
    }
        public void OnOpenInventory(InputValue value)
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        Cursor.lockState = inventoryUI.activeSelf? CursorLockMode.None : CursorLockMode.Locked;
        SpawnItems();
    }
    private void SpawnItems()
    {
        if (ItemSpawner.player == null)
        {
            ItemSpawner.player = gameObject.GetComponent<Player>();
        }
        ItemSpawner.itemStacks = stacksInInventory;
        ItemSpawner.SpawnItems();
    }
    public void RemoveDeadItems()
    {
        stacksInInventory.RemoveAll(x => x.state == ItemStack.StackState.Dead);
        ItemSpawner.RemoveDeadItems();
    }

    public void RemoveItem(ItemStack itemStack)
    {
        itemStack.state = ItemStack.StackState.Dead;
        RemoveDeadItems();
    }
    public void SwapPanels(ItemStack itemStack)
    {
        if (stacksInInventory.Contains(itemStack))
        {
            stacksInInventory.Remove(itemStack);
            stacksOnEquipmentPanel.Add(itemStack);
        }
        else
        {
            stacksOnEquipmentPanel.Remove(itemStack);
            stacksInInventory.Add(itemStack);
        }
        ItemSpawner.itemStacks = stacksInInventory;

    }
}
