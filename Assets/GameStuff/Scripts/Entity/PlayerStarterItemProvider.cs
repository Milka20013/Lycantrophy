using System;
using UnityEngine;

public class PlayerStarterItemProvider : MonoBehaviour, ISaveable
{
    [Serializable]
    public struct ItemSelector
    {
        public ItemBlueprint item;
        public int quantity;
    }

    [SerializeField] private ItemSelector[] itemSelectors;
    [SerializeField] private float money;
    private bool starterItemsAdded = false;

    private PlayerInventory inventory;
    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    private void Start()
    {
        if (!starterItemsAdded)
        {
            AddStarterItems();
        }
    }

    private void AddStarterItems()
    {
        for (int i = 0; i < itemSelectors.Length; i++)
        {
            inventory.AddItem(itemSelectors[i].item, itemSelectors[i].quantity);
        }
        inventory.AddCurrency(money);
        starterItemsAdded = true;
        GameDataManager.instance.SaveGame();
    }

    public void Save(ref GameData data)
    {
        data.starterItemsAdded = starterItemsAdded;
    }

    public void Load(GameData data)
    {
        starterItemsAdded = data.starterItemsAdded;
    }
}
