using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

//this class is for each individual itemslot in the inventory

public class ItemSlot : MonoBehaviour
{
    public int id = -1;

    [SerializeField] private ItemType acceptedItemType = ItemType.General;
    [HideInInspector] public Inventory inventory { get; set; }

    private RectTransform rectTransform;
    public Vector2 positionOffset;

    public GameObject attachedObject { get; set; }
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public bool OnItemDrop(GameObject item)
    {
        if (item == null)
        {
            return false;
        }
        ItemUI itemUI = item.GetComponent<ItemUI>();

        //if the slot can't accept the item
        if (!CanAcceptItem(itemUI.itemStack.item))
        {
            return false;
        }

        if (attachedObject != null)
        {
            //something is in the slot, so return. Swap items maybe?
            return false;
        }

        //invoke the placing procedure in code before placing the actual ui representation below
        itemUI.OnPlace(this);

        item.transform.SetParent(this.transform.parent);
        item.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition + positionOffset;
        attachedObject = item;
        return true;
    }

    public bool CanAcceptItem(Item item)
    {
        if (item == null)
        {
            return false;
        }
        if (acceptedItemType == ItemType.General)
        {
            return true;
        }
        return item.itemType == acceptedItemType;
    }
}
