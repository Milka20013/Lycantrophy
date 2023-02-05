using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

//this class is for each individual itemslot in the inventory

public class ItemSlot : MonoBehaviour
{
    public int id = -1;
    public enum ExpectedItemType { Any, Equippable, Consumable}
    public ExpectedItemType expectedItem = ExpectedItemType.Any;

    private RectTransform rectTransform;
    public Vector2 positionOffset;

    public GameObject attachedObject;



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnItemDrop(GameObject item)
    {
        if (attachedObject != null)
        {
            //something is in the slot, so return. Swap items maybe?
            return;
        }
        if (item != null)
        {
            item.transform.SetParent(this.transform.parent);
            item.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition + positionOffset;
            attachedObject = item;
        }
    }

    public bool CanEquipItem(Item item)
    {
        switch (expectedItem)
        {
            case ExpectedItemType.Any:
                return true;
            case ExpectedItemType.Equippable:
                return item.interactionType == InteractionType.Equip;
            case ExpectedItemType.Consumable:
                return item.interactionType == InteractionType.Consume;
            default:
                return true;
        }
    }

    public bool CanEquipItem(ItemStack itemStack)
    {
        return CanEquipItem(itemStack.item);
    }
    public bool CanEquipItem(ItemUI itemUI)
    {
        return CanEquipItem(itemUI.itemStack);
    }
}
