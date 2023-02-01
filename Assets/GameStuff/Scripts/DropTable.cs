using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{

    public DroppableItem[] droppableItems;
    [Serializable]
    public class DroppableItem
    {
        public ItemBlueprint item;
        public float percentage;
        public int[] itemRange;
    }
    public class DroppedItem
    {
        public ItemBlueprint item;
        public int quantity;
    }
    public void DropItems(Player playerWhoKilled)
    {
        DroppedItem[] droppedItems = new DroppedItem[droppableItems.Length];
        int[] itemRange = new int[2];
        for (int i = 0; i < droppableItems.Length; i++)
        {
            droppedItems[i] = new DroppedItem();
            droppedItems[i].item = droppableItems[i].item;
            if (droppableItems[i].itemRange.Length <= 1)
            {
                itemRange = new int[] {1,1};
            }
            else
            {
                itemRange = droppableItems[i].itemRange;
            }
            droppedItems[i].quantity = GameManager.GetRandomElementByPercentage(droppableItems[i].percentage, itemRange[0], itemRange[1]);
        }
        for (int i = 0; i < droppedItems.Length; i++)
        {
            playerWhoKilled.AddItem(droppedItems[i].item, droppedItems[i].quantity);
        }
    }
}
