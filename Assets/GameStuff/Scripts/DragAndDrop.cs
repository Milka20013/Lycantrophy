using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Tooltip("Name of the canvas on which the item is")]
    [SerializeField] private string canvasName;
    private Canvas canvas;

    public ItemUI itemUI;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public ItemSlot objectThisAttachedTo; //the itemslot which this item is on
    private Vector2 previousPosition;
    private Transform previousParent;

    public delegate void ItemDropManager(ItemUI itemUI, ItemSlot itemSlot);
    public ItemDropManager OnItemDropped;
    private void OnEnable()
    {
        FindCanvas();
    }
    private void Awake()
    {
        if (itemUI == null)
        {
            gameObject.TryGetComponent(out itemUI);
        }
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FindCanvas()
    {
        GameObject canv = GameObject.Find(canvasName);
        if (canv != null)
        {
            canvas = GameObject.Find(canvasName).GetComponent<Canvas>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        previousPosition = rectTransform.anchoredPosition;
        previousParent = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
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
                        itemslot.OnItemDrop(this.gameObject);
                        objectThisAttachedTo.attachedObject = null;
                        objectThisAttachedTo = itemslot;
                        foundObject = objects[i];
                        if (OnItemDropped != null)
                        {
                            OnItemDropped(itemUI, itemslot);
                        }
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
        canvasGroup.blocksRaycasts = true;
    }

    public void DropBack()
    {
        transform.SetParent(previousParent);
        rectTransform.anchoredPosition = previousPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
    private void OnDestroy()
    {
        objectThisAttachedTo.attachedObject = null;
    }

}
