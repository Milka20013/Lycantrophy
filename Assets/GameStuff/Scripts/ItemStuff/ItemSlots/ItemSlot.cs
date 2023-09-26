using UnityEngine;

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
        ItemUI droppedItemUI = item.GetComponent<ItemUI>();

        //selling the item. This should not be here i guess
        if (acceptedItemType == ItemType.None)
        {
            SellItem(droppedItemUI);
            return false;
        }

        //if the slot can't accept the item
        if (!CanAcceptItem(droppedItemUI.itemStack.item))
        {
            return false;
        }

        if (attachedObject != null)
        {
            //something is in the slot, so return false. Swap items maybe?
            ItemUI thisItemUI = attachedObject.GetComponent<ItemUI>();
            thisItemUI.OnDrop(droppedItemUI);
            return false;
        }

        //invoke the placing procedure in code before placing the actual ui representation below
        droppedItemUI.OnPlace(this);

        item.transform.SetParent(this.transform.parent);
        item.GetComponent<RectTransform>().anchoredPosition = ItemPosition();
        attachedObject = item;
        return true;
    }

    public Vector2 ItemPosition()
    {
        return rectTransform.anchoredPosition + positionOffset;
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

    private void SellItem(ItemUI itemUI)
    {
        itemUI.SellItem();
    }
}
