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
    private int activeTexts;
    public RectTransform rectTransform;

    [HideInInspector] public ItemUI currentItemUI;

    public void ShowPanel(ItemUI itemUI)
    {
        currentItemUI = itemUI;
        if (!itemDescriptionPanel.activeSelf)
        {
            UpdatePanel();
        }
        CalculatePivot(itemUI);
        rectTransform.position = itemUI.rectTransform.position;
        itemDescriptionPanel.SetActive(true);
    }

    private void CalculatePivot(ItemUI itemUI)
    {
        float x = 1;
        float y = 1;
        //the pivot changes based on the active effect texts. This ratio is a bit off
        y = itemUI.rectTransform.position.y <= canvasRectTransform.rect.width * 0.3f ?  (1 - (0.375f + (float)0.625f/effectTexts.Length * activeTexts)) : 1;
        x = itemUI.rectTransform.position.x <= canvasRectTransform.rect.width * 0.85f ? 0 : 1;
        rectTransform.pivot = new Vector2(x, y);
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
        for (int i = 0; i < currentItemUI.effectsDescription.Count; i++)
        {
            effectTexts[i].text = currentItemUI.effectsDescription[i];
            effectTexts[i].gameObject.SetActive(true);
        }
        activeTexts = currentItemUI.effectsDescription.Count;
    }
}
