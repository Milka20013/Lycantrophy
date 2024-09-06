
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject indicatorPosition;
    private bool isIndicatorShown;
    [HideInInspector] public Player player;
    IEnumerator levitate;

    public virtual void Awake()
    {
        levitate = LevitateIndicator();
    }
    public virtual void Interact(Player player)
    {
        this.player = player;
    }

    public virtual void Interact()
    {
        Debug.Log("Empty interact ");
    }

    public void ShowIndicator()
    {
        if (isIndicatorShown)
        {
            return;
        }
        isIndicatorShown = true;
        indicator.transform.position = indicatorPosition.transform.position;
        indicator.SetActive(true);
        StartCoroutine(levitate);
    }

    public virtual void HideIndicator()
    {
        if (!isIndicatorShown)
        {
            return;
        }
        isIndicatorShown = false;
        indicator.SetActive(false);
        StopCoroutine(levitate);
    }

    IEnumerator LevitateIndicator()
    {
        bool up = true;
        for (; ; )
        {
            if (up)
            {
                indicator.transform.Translate(0.2f * Time.deltaTime * Vector3.up);
                if (indicator.transform.position.y >= indicatorPosition.transform.position.y + 0.1f)
                {
                    up = false;
                }
            }
            else
            {
                indicator.transform.Translate(0.2f * Time.deltaTime * Vector3.down);
                if (indicator.transform.position.y <= indicatorPosition.transform.position.y - 0.1f)
                {
                    up = true;
                }
            }

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IndicatorManager.instance.ShowIndicator(indicatorPosition.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IndicatorManager.instance.HideIndicator(indicatorPosition.transform);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Interact(eventData.pressEventCamera.GetComponent<CameraInteract>().player);
    }
}
