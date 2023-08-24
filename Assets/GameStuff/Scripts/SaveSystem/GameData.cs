using Lycanthropy.Inventory;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{

    public PlayerData playerData;
    public AmplifierSystemData amplifierSystemData;
    public List<InventoryData> inventoryDatas;

    public GameData()
    {
        playerData = new PlayerData();
        inventoryDatas = new List<InventoryData>();
        amplifierSystemData = new AmplifierSystemData();
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
