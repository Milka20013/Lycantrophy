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
    public string id;
    public string itemName;
    public int stackSize = 1;
    public InteractionType interactionType;
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
