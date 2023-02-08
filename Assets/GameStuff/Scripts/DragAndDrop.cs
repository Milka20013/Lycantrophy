using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Tooltip("Mandatory if it is not an item")]
    [SerializeField] private Canvas canvas;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    
    private Vector2 previousPosition;
    private Transform previousParent;

    public void Init(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        previousPosition = rectTransform.anchoredPosition;
        previousParent = transform.parent;
        transform.SetParent(canvas.transform);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
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


}
