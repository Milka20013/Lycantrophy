using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private Player player;
    public ItemUI itemUI;
    public Amplifier[] amplifiers;
    private DragAndDrop dragAndDrop;
    private bool equipped = false;
    void Start()
    {
        player = itemUI.player;
        itemUI.SetInteractionType(InteractionType.Equip);
        if (gameObject.TryGetComponent(out dragAndDrop))
        {
            dragAndDrop.OnItemDropped += OnDrop;
        }
        itemUI.RegisterEffects(amplifiers);
    }
    public void OnDrop(ItemUI itemUI, ItemSlot itemSlot)
    { 
        if (itemSlot.expectedItem == ItemSlot.ExpectedItemType.Equippable)
        {
            if (player.playerInventory.ItemIsEquippedByName(itemUI.itemStack))
            {
                dragAndDrop.DropBack();
                return;
            }
            EquipItem();
        }
        else
        {
            UnequipItem();
        }
    }
    public void EquipItem()
    {
        equipped = true;
        player.playerInventory.AddItemToEquipmentPanel(itemUI.itemStack);
        player.playerStats.RegisterAmplifiers(amplifiers);
    }

    public void UnequipItem()
    {
        equipped = false;
        player.playerInventory.RemoveItemFromEquipmentPanel(itemUI.itemStack);
        player.playerStats.RemoveAmplifiers(amplifiers);
    }

    private void OnDestroy()
    {
        if (equipped)
        {
            UnequipItem();
        }
    }
}
