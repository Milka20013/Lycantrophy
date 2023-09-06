using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
[Serializable]
public class DroppableItem
{
    public ItemBlueprint item;
    public float percentage;
    public int[] itemRange;
}
public class DropTable : MonoBehaviour
{
    private HealthSystem healthSystem;
    private MobData mobData;

    [SerializeField] private Attribute luckAttribute;
    public class DroppedItem
    {
        public ItemBlueprint item;
        public int quantity;
    }
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        mobData = GetComponent<Stats>().entityData as MobData;
    }

    private void Start()
    {
        healthSystem.onDeath += OnDeath;
    }

    public void OnDeath(GameObject killer)
    {
        DropItems(killer);
        DropExp(killer);
        DropCurrency(killer);
    }
    public void DropItems(GameObject killer)
    {
        if (mobData.droppableItems == null)
        {
            return;
        }
        if (mobData.droppableItems.Count == 0)
        {
            return;
        }
        if (killer.TryGetComponent(out Inventory killerInv))
        {
            List<DroppableItem> droppableItems = mobData.droppableItems;
            droppableItems.AddRange(mobData.generalDrops);
            DroppedItem[] droppedItems = new DroppedItem[droppableItems.Count];
            int[] itemRange;
            float luck = 1;
            if (killer.TryGetComponent(out Stats stats))
            {
                stats.GetAttributeValue(luckAttribute, out luck);
            }
            for (int i = 0; i < droppableItems.Count; i++)
            {
                droppedItems[i] = new DroppedItem();
                droppedItems[i].item = droppableItems[i].item;
                if (droppableItems[i].itemRange.Length <= 1)
                {
                    itemRange = new int[] { 1, 1 };
                }
                else
                {
                    itemRange = droppableItems[i].itemRange;
                }
                droppedItems[i].quantity = Randomizer.GetDropByPercentage(droppableItems[i].percentage, luck, itemRange[0], itemRange[1]);
            }
            for (int i = 0; i < droppedItems.Length; i++)
            {
                killerInv.AddItem(droppedItems[i].item, droppedItems[i].quantity);
            }
        }
        else
        {
            return;
        }
    }

    public void DropExp(GameObject killer)
    {
        if (killer == null)
        {
            return;
        }
        if (killer.TryGetComponent(out Levelling scr))
        {
            scr.AddExp(mobData.rawExp);
        }
    }

    public void DropCurrency(GameObject killer)
    {
        if (killer == null)
        {
            return;
        }
        if (killer.TryGetComponent(out PlayerInventory inventory))
        {
            inventory.AddCurrency(mobData.rawCurrency);
        }
    }
}
