using System;
using System.Collections;
using System.Collections.Generic;
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
            public int itemId;
            public int quantity;
            public int slotId;
            public ItemData(int itemId, int quantity, int slotId)
            {
                this.itemId = itemId;
                this.quantity = quantity;
                this.slotId = slotId;
            }
        }
        public ItemData[] inventoryItems;
        public ItemData[] equipmentItems;

        public static ItemData[] CreateItemDatas(List<ItemStack> itemStacks)
        {
            ItemData[] itemDatas = new ItemData[itemStacks.Count];
            for (int i = 0; i < itemDatas.Length; i++)
            {
                itemDatas[i] = new ItemData(itemStacks[i].item.id, itemStacks[i].quantity, itemStacks[i].itemUI.slotId);
            }
            return itemDatas;
        }
    }

    public PlayerData playerData;
    public InventoryData inventoryData;
    public GameData()
    {
        playerData = new PlayerData();
        inventoryData = new InventoryData();
    }
}
