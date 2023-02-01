using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

//this class stores data related to one specific item

public enum InteractionType
{
    None, Consume, Use, Equip
}
[System.Serializable]
public class Item
{
    public int id;
    public string itemName;
    public int stackSize = 1;
    public int prefabId;
    public InteractionType interactionType;

    public Item(string itemName,int stackSize)
    {
        this.itemName = itemName;
        this.id = 0;
        this.stackSize = stackSize;
        this.prefabId = 0;
    }
    public Item(string itemName, int stackSize, int prefabId)
    {
        this.itemName = itemName;
        this.id = 0;
        this.stackSize = stackSize;
        this.prefabId = prefabId;
    }
    public Item(Item item)
    {
        this.itemName = item.itemName;
        this.id = item.id;
        this.stackSize = item.stackSize;
        this.prefabId = item.prefabId;
    }
    public Item(ItemBlueprint itemBlueprint)
    {
        this.itemName = itemBlueprint.itemName;
        this.id = itemBlueprint.id;
        this.stackSize = itemBlueprint.stackSize;
        this.prefabId = itemBlueprint.prefabId;
    }
}
