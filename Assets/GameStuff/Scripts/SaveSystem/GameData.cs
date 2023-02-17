using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameData 
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerData(Vector3 position)
        {
            this.position = position;
        }
        public Vector3 position;
    }
    [Serializable]
    public struct InventoryData
    {
        [Serializable]
        public struct ItemData
        {
            public string itemId;
            public int quantity;
            public int slotId;
            public ItemData(string itemId, int quantity, int slotId)
            {
                this.itemId = itemId;
                this.quantity = quantity;
                this.slotId = slotId;
            }
        }
        public string id;
        public ItemData[] inventoryItems;

        public static ItemData[] CreateItemDatas(List<ItemStack> itemStacks)
        {
            ItemData[] itemDatas = new ItemData[itemStacks.Count];
            for (int i = 0; i < itemDatas.Length; i++)
            {
                itemDatas[i] = new ItemData(itemStacks[i].item.id, itemStacks[i].quantity, itemStacks[i].itemUI == null? -1 : itemStacks[i].itemUI.slotId);
            }
            return itemDatas;
        }

        public InventoryData(ItemData[] inventoryItems, string id)
        {
            this.id = id;
            this.inventoryItems = inventoryItems;
        }
    }

    public PlayerData playerData;
    public List<InventoryData> inventoryDatas;

    public GameData()
    {
        playerData = new PlayerData();
        inventoryDatas = new List<InventoryData>();
    }

    public int GetIndex(string id)
    {
        for (int i = 0; i < inventoryDatas.Count; i++)
        {
            if (inventoryDatas[i].id == "" || inventoryDatas[i].id == id)
            {
                return i;
            }
        }
        return -1;
    }
}
