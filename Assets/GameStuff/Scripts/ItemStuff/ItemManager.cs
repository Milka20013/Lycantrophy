using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemManager",menuName ="Manager/ItemManager")]
public class ItemManager : ScriptableSingleton<ItemManager>
{
    private ItemBlueprint[] itemBlueprints;
    [SerializeField] private EquipmentItem[] equipmentItems;
    [SerializeField] private ConsumableItem[] consumableItems;
    private Item[] items;

    private void Awake()
    {
        RefreshData();
    }
    public void RefreshData()
    {
        FindItems();

        itemBlueprints = new ItemBlueprint[equipmentItems.Length + consumableItems.Length];
        int j = 0;
        for (int i = 0; i < equipmentItems.Length; i++)
        {
            itemBlueprints[i] = equipmentItems[i];
            j++;
        }
        for (int i = 0; i < consumableItems.Length; i++)
        {
            itemBlueprints[j + i] = consumableItems[i];
        }
        items = new Item[itemBlueprints.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(itemBlueprints[i]);
        }
    }
    private void FindItems()
    {
        equipmentItems = Resources.LoadAll<EquipmentItem>("ItemBlueprints/Equipment");
        consumableItems = Resources.LoadAll<ConsumableItem>("ItemBlueprints/Consumable");
    }

    
    public Item GetItem(string id)
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

    public ItemBlueprint GetItemBlueprint(ItemStack item)
    {
        ItemBlueprint itemBp = null;
        for (int i = 0; i < itemBlueprints.Length; i++)
        {
            if (item.item.id == itemBlueprints[i].id)
            {
                itemBp = itemBlueprints[i];
            }
        }
        return itemBp;
    }
    public EquipmentItem GetEquipmentItemBlueprint(ItemStack item)
    {
        EquipmentItem itemBp = null;
        for (int i = 0; i < equipmentItems.Length; i++)
        {
            if (item.item.id == equipmentItems[i].id)
            {
                itemBp = equipmentItems[i];
            }
        }
        return itemBp;
    }
    public ConsumableItem GetConsumableItemBlueprint(ItemStack item)
    {
        ConsumableItem itemBp = null;
        for (int i = 0; i < consumableItems.Length; i++)
        {
            if (item.item.id == consumableItems[i].id)
            {
                itemBp = consumableItems[i];
            }
        }
        return itemBp;
    }
}
