using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemManager", menuName = "Manager/ItemManager")]
public class ItemManager : ScriptableObject
{
    private List<ItemBlueprint> itemBlueprints = new();
    [SerializeField] private OrbBlueprint[] orbBlueprints;
    [SerializeField] private ConsumableBlueprint[] consumableItems;
    [SerializeField] private ProductBlueprint[] productItems;
    [SerializeField] private ItemBlueprint[] miscItems;
    private Item[] items;

    private void Awake()
    {
        RefreshData();
    }
    public void RefreshData()
    {
        FindItems();
        itemBlueprints.AddRange(orbBlueprints);
        itemBlueprints.AddRange(consumableItems);
        itemBlueprints.AddRange(miscItems);
        itemBlueprints.AddRange(productItems);
        items = new Item[itemBlueprints.Count];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item(itemBlueprints[i]);
        }
    }
    private void FindItems()
    {
        orbBlueprints = Resources.LoadAll<OrbBlueprint>("ItemBlueprints/Equipment");
        consumableItems = Resources.LoadAll<ConsumableBlueprint>("ItemBlueprints/Consumable");
        productItems = Resources.LoadAll<ProductBlueprint>("ItemBlueprints/Product");
        miscItems = Resources.LoadAll<ItemBlueprint>("ItemBlueprints/Misc");
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
        for (int i = 0; i < itemBlueprints.Count; i++)
        {
            if (item.item.id == itemBlueprints[i].id)
            {
                return itemBlueprints[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
    public OrbBlueprint GetEquipmentItemBlueprint(ItemStack item)
    {
        for (int i = 0; i < orbBlueprints.Length; i++)
        {
            if (item.item.id == orbBlueprints[i].id)
            {
                return orbBlueprints[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
    public ConsumableBlueprint GetConsumableItemBlueprint(ItemStack item)
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

    public ProductBlueprint GetProductItemBlueprint(ItemStack item)
    {
        for (int i = 0; i < productItems.Length; i++)
        {
            if (item.item.id == productItems[i].id)
            {
                return productItems[i];
            }
        }
        Debug.LogError("ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
}
