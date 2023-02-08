using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI quantity;

    [HideInInspector] public Inventory inventory { get; private set; }

    [HideInInspector] public Player player { get; private set; }


    [HideInInspector] public ItemStack itemStack { get; private set; }

    [HideInInspector] public int slotId { get; private set; }

    [HideInInspector] public ItemBlueprint itemBlueprint { get; private set; }


    [HideInInspector] public List<string> effects { get; private set; } = new List<string>();

    private ItemDescriptionPanel itemDescriptionPanel;
    public void SetReferences(Inventory inventory, ItemDescriptionPanel itemDescriptionPanel, ItemBlueprint itemBP, Player player)
    {
        this.inventory = inventory;
        this.itemDescriptionPanel = itemDescriptionPanel;
        this.itemBlueprint = itemBP;
        this.player = player;
    }
    public void SetItemInfos(Vector2 position, ItemStack itemStack, int itemSlotId)
    {
        rectTransform.anchoredPosition = position;
        this.itemStack = itemStack;
        itemStack.itemUI = this;
        itemStack.ChangeQuantity(itemStack.quantity);
        slotId = itemSlotId;        
    }

    public void SetInteractionType(InteractionType interactionType)
    {
        itemStack.item.interactionType = interactionType;
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            inventory.DeleteItem(itemStack);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDescriptionPanel.ShowPanel(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescriptionPanel.HidePanel();
    }

    public void OnDrop(ItemSlot slot)
    {
        slotId = slot.id;
        inventory.ChangeInventories(itemStack, slot.inventory);
        inventory = slot.inventory;
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
