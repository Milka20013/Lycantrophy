using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//this class spawns the items onto the inventoryUI

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public ItemDescriptionPanel itemDescriptionPanelScr;
    public Inventory inventory;
    public List<ItemStack> itemStacks { get; set; }
    public ItemSlot[] slots;

    public HashSet<ItemStack> itemsOnInventory = new HashSet<ItemStack>();
    [HideInInspector] public List<ItemUI> spawnedItems = new List<ItemUI>();

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].inventory = inventory;
        }
    }

    public void InstanstiateItem(ItemStack itemStack,int itemSlotId)
    {
        if (itemSlotId == -1)
        {
            itemSlotId = GetFirstFreeItemSlotId();
        }
        if (itemSlotId == -1)
        {
            return;
        }
        ItemSlot itemSlot = slots[itemSlotId];

        RectTransform rectTransform = itemSlot.gameObject.GetComponent<RectTransform>();

        //position of the itemstack
        rectTransform = itemSlot.gameObject.GetComponent<RectTransform>();

        //spawning
        GameObject objectToSpawn = GetItemToSpawn(itemStack);
        GameObject item = Instantiate(objectToSpawn, rectTransform.anchoredPosition, rectTransform.rotation);
        item.transform.SetParent(gameObject.transform, false);

        //setting the infos to itemUI, so it can dynamically change later
        ItemUI itemUI = item.GetComponent<ItemUI>();
        itemUI.SetItemInfos(rectTransform.anchoredPosition, itemStack, itemSlotId);

        itemUI.SetReferences(inventory, itemDescriptionPanelScr, inventory.itemManager.GetItemBlueprint(itemStack),inventory.player);

        //assigning the item to the slot, so it will drag and drop properly
        item.GetComponent<DragAndDropItem>().Init(canvas,itemSlot);
        itemSlot.attachedObject = item;

        //add to the list, so we can remove later
        spawnedItems.Add(item.GetComponent<ItemUI>());

        //assigning the item to the UI inventory, so we will not add it again
        itemsOnInventory.Add(itemStack);
    }
    public void SpawnItems()
    {
        for (int i = 0; i < itemStacks.Count; i++)
        {
            //if the item is on the inventory, we don't do anything
            if (itemsOnInventory.TryGetValue(itemStacks[i], out _))
            {
                continue;
            }

            int index = GetFirstFreeItemSlotId();
            if (index == -1)
            {
                return;
            }
            InstanstiateItem(itemStacks[i], index);
        }
    }
    private GameObject GetItemToSpawn(ItemStack itemStack)
    {
        return itemStack.item.prefab;
    }
    public int GetFirstFreeItemSlotId()
    {
        int index = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].attachedObject == null)
            {
                return i;
            }
        }
        return index;
    }
    public void DeleteDeadItems()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i].itemStack.state == ItemStack.StackState.Dead)
            {
                Destroy(spawnedItems[i].gameObject);
                spawnedItems[i] = null;
            }
        }
        for (int i = 0; i < itemStacks.Count; i++)
        {
            if (itemStacks[i].state == ItemStack.StackState.Dead)
            {
                itemStacks[i] = null;
            }
        }
        spawnedItems.RemoveAll(x => x == null);
        itemStacks.RemoveAll(x => x == null);
        itemsOnInventory.RemoveWhere(x=>x.state == ItemStack.StackState.Dead);
    }
}
