using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    public GameObject itemDescriptionPanel;
    public RectTransform canvasRectTransform;
    public TextMeshProUGUI basicDescription;
    public TextMeshProUGUI[] effectTexts;
    public RectTransform rectTransform;

    [HideInInspector] public ItemUI currentItemUI;

    public void ShowPanel(ItemUI itemUI)
    {
        currentItemUI = itemUI;
        if (itemUI.rectTransform.position.x >= canvasRectTransform.rect.width * 0.85f)
        {
            rectTransform.pivot = new Vector2(1,1);
        }
        else
        {
            rectTransform.pivot = new Vector2(0, 1);
        }
        rectTransform.position = itemUI.rectTransform.position;
        if (!itemDescriptionPanel.activeSelf)
        {
            UpdatePanel();
        }
        itemDescriptionPanel.SetActive(true);
    }

    public void HidePanel()
    {
        itemDescriptionPanel.SetActive(false);
    }

    public void UpdatePanel()
    {
        basicDescription.text = currentItemUI.itemBlueprint.basicDescription;
        for (int i = 0; i < effectTexts.Length; i++)
        {
            effectTexts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentItemUI.effects.Count; i++)
        {
            effectTexts[i].text = currentItemUI.effects[i];
            effectTexts[i].gameObject.SetActive(true);
        }
    }
}
