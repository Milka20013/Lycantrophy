using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragAndDropItem : DragAndDrop
{
    public ItemUI itemUI;
    [HideInInspector] public ItemSlot slotThisAttachedTo { get; set; } //the itemslot which this item is on

    public delegate bool ItemDropManager(ItemUI itemUI, ItemSlot itemSlot);
    public ItemDropManager OnItemDropped;
    public void Init(Canvas canvas, ItemSlot itemSlot)
    {
        base.Init(canvas);
        slotThisAttachedTo = itemSlot;
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Any())
        {
            List<GameObject> objects = eventData.hovered;
            GameObject foundObject = null;
            //find a slot
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].TryGetComponent(out ItemSlot itemslot) && itemslot.CanEquipItem(itemUI))
                {
                    if (itemslot.attachedObject == null)
                    {
                        if (OnItemDropped != null)
                        {
                            if (!OnItemDropped(itemUI, itemslot))
                            {
                                break;
                            }
                        }

                        itemUI.OnDrop(itemslot);

                        itemslot.OnItemDrop(this.gameObject);




                        slotThisAttachedTo.attachedObject = null;
                        slotThisAttachedTo = itemslot;


                        foundObject = objects[i];
                        break;
                    }
                }
            }
            if (foundObject == null)
            {
                DropBack();
            }
        }
        else
        {
            DropBack();
        }
        base.OnEndDrag(eventData);
    }
    private void OnDestroy()
    {
        slotThisAttachedTo.attachedObject = null;
    }
}
