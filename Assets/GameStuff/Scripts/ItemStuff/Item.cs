using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class stores data related to one specific item
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

public enum ItemType
{
    General, Consumable, Useable, Orb, Essence
}
[System.Serializable]
public class Item
{
    public string id;
    public string itemName;
    public int stackSize = 1;
    public ItemType itemType;
    public GameObject prefab;
    public Item(Item item)
    {
        this.itemName = item.itemName;
        this.id = item.id;
        this.stackSize = item.stackSize;
        this.prefab = item.prefab;
    }
    public Item(ItemBlueprint itemBlueprint)
    {
        this.itemName = itemBlueprint.itemName;
        this.id = itemBlueprint.id;
        this.stackSize = itemBlueprint.stackSize;
        this.prefab = itemBlueprint.prefab;
    }
}
