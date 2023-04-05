using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetDescriptionInspector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SetBonusDescription setBonusDescription;
    [SerializeField] private TextMeshProUGUI tip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (setBonusDescription.inv.setbonus == null)
        {
            tip.gameObject.SetActive(true);
            return;
        }
        setBonusDescription.ShowPanel();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tip.gameObject.SetActive(false);
        setBonusDescription.HidePanel();
    }
}
