using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private Player player;
    public ItemUI itemUI;
    public Amplifier[] amplifiers;
    void Start()
    {
        player = itemUI.player;
        itemUI.SetInteractionType(InteractionType.Equip);
        if (gameObject.TryGetComponent(out DragAndDrop dragAndDrop))
        {
            dragAndDrop.OnItemDropped += OnDrop;
        }
        itemUI.RegisterEffects(amplifiers);
    }
    public void OnDrop(ItemUI itemUI, ItemSlot itemSlot)
    {
        if (itemSlot.expectedItem == ItemSlot.ExpectedItemType.Equippable)
        {
            EquipItem();
        }
        else
        {
            UnequipItem();
        }
    }
    public void EquipItem()
    {
        player.playerInventory.SwapPanels(itemUI.itemStack);
        player.playerStats.RegisterAmplifiers(amplifiers);
    }

    public void UnequipItem()
    {
        player.playerInventory.SwapPanels(itemUI.itemStack);
        player.playerStats.RemoveAmplifiers(amplifiers);
    }

}
