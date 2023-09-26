using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Tooltip("Mandatory if it is not an item")]
    [SerializeField] private Canvas canvas;
    private int initialSortingOrder;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] protected RectTransform rectTransform;


    protected Vector2 previousPosition;
    private Transform previousParent;

    public void Init(Canvas canvas)
    {
        this.canvas = canvas;
        initialSortingOrder = canvas.sortingOrder;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        previousPosition = rectTransform.anchoredPosition;
        previousParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvas.sortingOrder = 10;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
    }

    public void DropBack()
    {
        if (!previousParent.gameObject.activeSelf)
        {
            return;
        }
        transform.SetParent(previousParent);
        rectTransform.anchoredPosition = previousPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    protected virtual void EndDrag()
    {
        canvasGroup.blocksRaycasts = true;
        canvas.sortingOrder = initialSortingOrder;
        if (IsOutsideOfView())
        {
            if (previousParent == transform.parent)
            {
                rectTransform.anchoredPosition = previousPosition;
            }
        }
    }

    protected bool IsOutsideOfView()
    {
        if (Mathf.Abs(rectTransform.anchoredPosition.x * 2) > Screen.width)
        {
            return true;
        }
        if (Mathf.Abs(rectTransform.anchoredPosition.y * 2) > Screen.height)
        {
            return true;
        }
        return false;
    }

    private void OnDisable()
    {
        EndDrag();
    }


}
