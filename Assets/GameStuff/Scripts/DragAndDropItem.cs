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
                if (objects[i].TryGetComponent(out ItemSlot itemSlot))
                {
                    if (!itemSlot.OnItemDrop(this.gameObject))
                    {
                        break;
                    }
                    slotThisAttachedTo.attachedObject = null;
                    slotThisAttachedTo = itemSlot;
                    foundObject = objects[i];
                    break;
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
