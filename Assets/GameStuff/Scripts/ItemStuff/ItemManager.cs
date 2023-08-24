using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemManager", menuName = "Manager/ItemManager")]
public class ItemManager : ScriptableObject
{
    private Dictionary<string, ItemBlueprint> itemBlueprintsDict = new();
    [SerializeField] private OrbBlueprint[] orbBlueprints;
    [SerializeField] private ConsumableBlueprint[] consumableBlueprints;
    [SerializeField] private ProductBlueprint[] productBlueprints;
    [SerializeField] private ItemBlueprint[] miscBlueprints;
    [SerializeField] private EssenceBlueprint[] essenceBlueprints;
    [SerializeField] private CorePowerModifierBlueprint[] corePowerModifierBlueprints;

    private List<Item> items = new();


    private void Awake()
    {
        RefreshData();
    }
    public void RefreshData()
    {
        FindItems();
        AddRangeToDict(orbBlueprints);
        AddRangeToDict(consumableBlueprints);
        AddRangeToDict(miscBlueprints);
        AddRangeToDict(productBlueprints);
        AddRangeToDict(essenceBlueprints);
        AddRangeToDict(corePowerModifierBlueprints);
        items.Clear();
        foreach (KeyValuePair<string, ItemBlueprint> keyValue in itemBlueprintsDict)
        {
            items.Add(new Item(keyValue.Value));
        }
    }

    public void ClearData()
    {
        itemBlueprintsDict.Clear();
    }

    private void AddRangeToDict(IEnumerable<ItemBlueprint> items)
    {
        foreach (var item in items)
        {
            if (!itemBlueprintsDict.TryAdd(item.id, item) && itemBlueprintsDict[item.id] != item)
            {
                Debug.LogError($"{item} Could not be added to ItemBlueprint dict with id {item.id}");
            }
        }
    }
    private void FindItems()
    {
        orbBlueprints = Resources.LoadAll<OrbBlueprint>("ItemBlueprints/Equipment/Orb");
        essenceBlueprints = Resources.LoadAll<EssenceBlueprint>("ItemBlueprints/Equipment/Essence");
        corePowerModifierBlueprints = Resources.LoadAll<CorePowerModifierBlueprint>("ItemBlueprints/Misc/CorePowerModifier");
        consumableBlueprints = Resources.LoadAll<ConsumableBlueprint>("ItemBlueprints/Consumable");
        productBlueprints = Resources.LoadAll<ProductBlueprint>("ItemBlueprints/Product");
        miscBlueprints = Resources.LoadAll<ItemBlueprint>("ItemBlueprints/Misc");
    }


    public Item GetItem(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
                return new Item(items[i]);
            }
        }
        Debug.LogError("Item was not registered.");
        return null;
    }
    public Item GetItem(ItemBlueprint item)
    {
        return new Item(item);
    }
    public Item GetItemByName(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == name)
            {
                return new Item(items[i]);
            }
        }
        Debug.LogError("Item was not registered.");
        return null;
    }


    public T GetItemBlueprint<T>(ItemStack item, bool recursive = true) where T : class
    {
        if (itemBlueprintsDict.ContainsKey(item.item.id))
        {
            return itemBlueprintsDict[item.item.id] as T;
        }
        if (recursive)
        {
            RefreshData();
            Debug.Log($"Attempting to find the item {item.item.itemName} (this message should only appear once !)");
            return GetItemBlueprint<T>(item, false);
        }
        Debug.LogError($"{item.item.itemName} ItemBP was not registered. Try finding BPs in ItemManagerSO");
        return null;
    }
}
