using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemUI))]
[RequireComponent(typeof(DragAndDropItem))]
public class Equipment : MonoBehaviour
{
    public ItemUI itemUI;
    public DragAndDropItem dragAndDrop;
    private EquipmentItem equipmentItem;
    private bool equipped = false;
    void Start()
    {
        equipmentItem = itemUI.inventory.itemManager.GetEquipmentItemBlueprint(itemUI.itemStack);
        itemUI.SetInteractionType(InteractionType.Equip);
        if (gameObject.TryGetComponent(out dragAndDrop))
        {
            dragAndDrop.OnItemDropped += OnDrop;
        }
        itemUI.RegisterEffects(equipmentItem.amplifiers);
        if (dragAndDrop.slotThisAttachedTo.inventory.inventoryTag == Inventory.InventoryTag.Equipment)
        {
            itemUI.player.playerStats.RegisterAmplifiers(equipmentItem.amplifiers);
        }
    }
    public bool OnDrop(ItemUI itemUI, ItemSlot itemSlot)
    {
        bool equip = itemSlot.inventory.inventoryTag == Inventory.InventoryTag.Equipment
                   && dragAndDrop.slotThisAttachedTo.inventory.inventoryTag != Inventory.InventoryTag.Equipment;

        bool unequip = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment 
                   && dragAndDrop.slotThisAttachedTo.inventory.inventoryTag == Inventory.InventoryTag.Equipment;

        //bool nothing = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment;
        if (itemSlot.expectedItem == ItemSlot.ExpectedItemType.Equippable && equip)
        {
            if (itemUI.player.equipmentInventory.ItemIsEquippedByName(itemUI.itemStack))
            {
                return false;
            }
            EquipItem();
        }
        else if (unequip)
        {
            UnequipItem();
        }
        return true;
    }
    public void EquipItem()
    {
        equipped = true;
        itemUI.player.playerInventory.EquipItem(itemUI.itemStack);
        itemUI.player.playerStats.RegisterAmplifiers(equipmentItem.amplifiers);
    }

    public void UnequipItem()
    {
        equipped = false;
        itemUI.player.equipmentInventory.UnequipItem(itemUI.itemStack);
        itemUI.player.playerStats.UnRegisterAmplifiers(equipmentItem.amplifiers);
    }

    private void OnDestroy()
    {
        if (equipped)
        {
            UnequipItem();
        }
    }
}
