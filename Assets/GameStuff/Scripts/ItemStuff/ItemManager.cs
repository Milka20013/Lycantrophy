using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemManager",menuName ="Manager/ItemManager")]
public class ItemManager : ScriptableObject
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
        for (int i = 0; i < itemBlueprints.Length; i++)
        {
            if (item.item.id == itemBlueprints[i].id)
            {
                return itemBlueprints[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
    public EquipmentItem GetEquipmentItemBlueprint(ItemStack item)
    {
        for (int i = 0; i < equipmentItems.Length; i++)
        {
            if (item.item.id == equipmentItems[i].id)
            {
                return equipmentItems[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
    public ConsumableItem GetConsumableItemBlueprint(ItemStack item)
    {
        for (int i = 0; i < consumableItems.Length; i++)
        {
            if (item.item.id == consumableItems[i].id)
            {
                return consumableItems[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
}
