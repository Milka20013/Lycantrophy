using System;
using System.Collections;
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
    public HealthSystem healthSystem;
    [HideInInspector] public MobData mobData;

    public class DroppedItem
    {
        public ItemBlueprint item;
        public int quantity;
    }

    private void Start()
    {
        healthSystem.onDeath += DropExp;
        healthSystem.onDeath += DropItems;
    }
    public void DropItems(GameObject killer)
    {
        if (killer.TryGetComponent(out Inventory killerInv))
        {
            List<DroppableItem> droppableItems = mobData.droppableItems;
            droppableItems.AddRange(mobData.generalDrops);
            DroppedItem[] droppedItems = new DroppedItem[droppableItems.Count];
            int[] itemRange = new int[2];
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
                droppedItems[i].quantity = GameManager.GetRandomElementByPercentage(droppableItems[i].percentage, itemRange[0], itemRange[1]);
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
        killer.GetComponent<Levelling>()?.AddExp(mobData.rawExp);
    }
}
