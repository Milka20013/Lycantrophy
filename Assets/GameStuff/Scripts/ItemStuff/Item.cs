using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public InteractionType interactionType;
    public ItemType itemType;

    public Item(string itemName,int stackSize)
    {
        this.itemName = itemName;
        this.id = 0;
        this.stackSize = stackSize;
        this.itemType = ItemType.None;
    }
    public Item(string itemName, int stackSize, ItemType itemType)
    {
        this.itemName = itemName;
        this.id = 0;
        this.stackSize = stackSize;
        this.itemType = itemType;
    }
    public Item(Item item)
    {
        this.itemName = item.itemName;
        this.id = item.id;
        this.stackSize = item.stackSize;
        this.itemType = item.itemType;
    }
    public Item(ItemBlueprint itemBlueprint)
    {
        this.itemName = itemBlueprint.itemName;
        this.id = itemBlueprint.id;
        this.stackSize = itemBlueprint.stackSize;
        this.itemType = itemBlueprint.itemType;
    }
}
