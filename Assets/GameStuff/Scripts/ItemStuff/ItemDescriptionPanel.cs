using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    public static ItemDescriptionPanel instance;

    [SerializeField] private GameObject itemDescriptionContainer;
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI basicDescription;
    [SerializeField] private Transform effectsContainer;
    private RectTransform canvasRectTransform;

    private List<TextMeshProUGUI> effectTexts = new();
    private int numberOfActiveTexts;


    [SerializeField] private TextMeshProUGUI effectTextPrefab;
    private float textHeight;
    private float spacing;

    private Vector2 initialBackGroundOffsetMin;

    [HideInInspector] public ItemUI currentItemUI;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of " + name);
        }
        instance = this;
        initialBackGroundOffsetMin = background.offsetMin;
        canvasRectTransform = effectsContainer.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        textHeight = effectTextPrefab.GetComponent<RectTransform>().rect.height;
        spacing = effectsContainer.GetComponent<VerticalLayoutGroup>().spacing;
    }
    public void ShowPanel(ItemUI itemUI)
    {
        currentItemUI = itemUI;
        if (!itemDescriptionContainer.activeSelf)
        {
            UpdatePanel();
        }
        AdjustBackground();
        AdjustPosition(itemUI.rectTransform.position);
        itemDescriptionContainer.SetActive(true);
    }

    /// <summary>
    /// Positions the panel such a way that the backgorund's center is at the line of the origin.
    /// The side is shifted by a small amount (the item's size)
    /// </summary>
    private void AdjustPosition(Vector3 origin)
    {
        float offsetY = background.transform.position.y - panelRectTransform.transform.position.y;
        float offsetx = background.rect.width / 2 + 35;

        float posX = origin.x > canvasRectTransform.rect.width * 0.5f ? origin.x - offsetx : origin.x + offsetx;
        float posY = origin.y - offsetY;

        panelRectTransform.position = new Vector3(posX, posY, 0);

        //This is preventing the panel to clip off the screen vertically
        if (background.rect.height >= canvasRectTransform.rect.height)
        {
            return;
        }
        float differenceUp = background.position.y + background.rect.height / 2 - canvasRectTransform.rect.height;
        if (differenceUp > 0)
        {
            posY -= differenceUp;
        }
        float differenceDown = background.position.y - background.rect.height / 2;
        if (differenceDown < 0)
        {
            posY -= differenceDown;
        }
        panelRectTransform.position = new Vector3(posX, posY, 0);

    }

    private void AdjustBackground()
    {
        float offset = numberOfActiveTexts * (textHeight + spacing);
        background.offsetMin = new Vector2(initialBackGroundOffsetMin.x, initialBackGroundOffsetMin.y - offset);
    }
    public void HidePanel(ItemUI itemUI)
    {
        if (itemUI == currentItemUI)
        {
            HidePanel();
        }
    }
    public void HidePanel()
    {
        itemDescriptionContainer.SetActive(false);
    }

    public void UpdatePanel()
    {
        string description = currentItemUI.basicDescription.Replace("<br>", "\n");
        basicDescription.text = description;

        itemName.text = currentItemUI.itemStack.item.itemName;

        effectsContainer.gameObject.SetActive(true);

        PopulateEffectTexts(currentItemUI.effectsDescription.Count);

        RefreshEffectTexts();

        numberOfActiveTexts = currentItemUI.effectsDescription.Count;
        if (numberOfActiveTexts == 0)
        {
            effectsContainer.gameObject.SetActive(false);
        }
    }

    private void RefreshEffectTexts()
    {
        for (int i = 0; i < effectTexts.Count; i++)
        {
            effectTexts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentItemUI.effectsDescription.Count; i++)
        {
            if (i >= effectTexts.Count)
            {
                break;
            }
            effectTexts[i].text = currentItemUI.effectsDescription[i];
            effectTexts[i].gameObject.SetActive(true);
        }
    }

    private void PopulateEffectTexts(int numberOfTexts)
    {
        int difference = numberOfTexts - effectTexts.Count;
        for (int i = 0; i < difference; i++)
        {
            var text = Instantiate(effectTextPrefab, effectsContainer.position, quaternion.identity, effectsContainer);
            effectTexts.Add(text);
        }
    }
}
