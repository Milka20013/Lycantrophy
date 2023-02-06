using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;
    public TextMeshProUGUI quantity;

    [HideInInspector] public Inventory inventory;

    [HideInInspector] public Player player;

    public ItemDescriptionPanel itemDescriptionPanel;

    [HideInInspector] public ItemStack itemStack;

    [HideInInspector] public int slotId;


    public string basicDescription;
    [HideInInspector] public List<string> effects;
    public void SetInfos(Vector2 position, ItemStack itemStack, Inventory inventory, int itemSlotId)
    {
        rectTransform.anchoredPosition = position;
        this.itemStack = itemStack;
        itemStack.itemUI = this;
        itemStack.ChangeQuantity(itemStack.quantity);
        this.inventory = inventory;
        slotId = itemSlotId;
    }

    public void SetInteractionType(InteractionType interactionType)
    {
        itemStack.item.interactionType = interactionType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            inventory.RemoveItem(itemStack);
        }
    }

    public void ChangeQuantityText(float amount)
    {
        if (amount <= 1)
        {
            quantity.text = "";
        }
        else
        {
            quantity.text = amount.ToString();
        }
    }

    public void RegisterEffects(Amplifier[] amplifiers)
    {
        for (int i = 0; i < amplifiers.Length; i++)
        {
            effects.Add(amplifiers[i].Description());
        }
    }
    public void RegisterEffects(string desc)
    {
        effects.Add(desc);
    }
    public void RegisterEffects(string[] desc)
    {
        effects.AddRange(desc);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDescriptionPanel.ShowPanel(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescriptionPanel.HidePanel();
    }

    private void OnDestroy()
    {
        if (itemDescriptionPanel == null)
        {
            return;
        }
        itemDescriptionPanel.HidePanel();
    }
}
