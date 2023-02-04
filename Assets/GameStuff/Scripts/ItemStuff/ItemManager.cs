using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//replace this enum with the generated if itemsData changes

public class ItemManager : MonoBehaviour
{
    [Tooltip("Only needs to be set if a script references the items via name or id")]
    public ItemBlueprint[] itemBlueprints;
    private Item[] items;

    private void Awake()
    {
        items = new Item[itemBlueprints.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(itemBlueprints[i]);
        }
    }
    public Item GetItem(string name)
    {
        Item item = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == name)
            {
                item = new Item(items[i]);
            }
        }
        return item;
    }
    public Item GetItem(int id)
    {
        Item item = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].id == id)
            {
                item = new Item(items[i]);
            }
        }
        return item;
    }
    public Item GetItem(ItemBlueprint item)
    {
        return new Item(item);
    }
}
