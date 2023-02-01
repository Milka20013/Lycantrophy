using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//this class spawns the items onto the inventoryUI

public class ItemSpawner : MonoBehaviour
{
    [HideInInspector] public Player player;
    public List<ItemStack> itemStacks = new List<ItemStack>();
    public ItemSlot[] slots;
    public GameObject[] itemPrefabs;

    public HashSet<ItemStack> itemsOnInventory = new HashSet<ItemStack>();
    public List<ItemUI> spawnedItems = new List<ItemUI>();
    public void SpawnItems()
    {
        RectTransform rectTransfrom;
        for (int i = 0; i < itemStacks.Count; i++)
        {
            //if the item is on the inventory, we don't do anything
            if (itemsOnInventory.TryGetValue(itemStacks[i], out _))
            {
                continue;
            }

            //get the first free  itemslot
            ItemSlot itemSlot = GetFirstFreeItemSlot();
            if (itemSlot == null)
            {
                return;
            }

            //position of the itemstack
            rectTransfrom = itemSlot.gameObject.GetComponent<RectTransform>();

            //spawning
            GameObject objectToSpawn = itemPrefabs[itemStacks[i].item.prefabId];
            GameObject item = Instantiate(objectToSpawn, rectTransfrom.anchoredPosition, rectTransfrom.rotation);
            item.transform.SetParent(gameObject.transform);

            //setting the infos to itemUI, so it can dynamically change later
            ItemUI itemUI = item.GetComponent<ItemUI>();
            itemUI.SetInfos(rectTransfrom.anchoredPosition, itemStacks[i], player);

            //assigning the item to the slot, so it will drag and drop properly
            item.GetComponent<DragAndDrop>().objectThisAttachedTo = itemSlot;
            itemSlot.attachedObject = item;

            //add to the list, so we can remove later
            spawnedItems.Add(item.GetComponent<ItemUI>()); 

            //assigning the item to the UI inventory, so we will not add it again
            itemsOnInventory.Add(itemStacks[i]);
        }
    }
    public ItemSlot GetFirstFreeItemSlot()
    {
        ItemSlot itemSlot = null; 
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].attachedObject == null)
            {
                return slots[i];
            }
        }
        return itemSlot;
    }
    public void RemoveDeadItems()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i].itemStack.state == ItemStack.StackState.Dead)
            {
                Destroy(spawnedItems[i].gameObject);
                spawnedItems[i] = null;
            }
        }
        spawnedItems.RemoveAll(x => x == null);
        itemsOnInventory.RemoveWhere(x=>x.state == ItemStack.StackState.Dead);
    }
}
