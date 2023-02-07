using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private Player player;
    public ItemUI itemUI;
    public EquipmentItem equipmentItem;
    public DragAndDrop dragAndDrop;
    private bool equipped = false;
    void Start()
    {
        player = itemUI.player;
        equipmentItem = itemUI.inventory.itemManager.GetEquipmentItemBlueprint(itemUI.itemStack);
        itemUI.SetInteractionType(InteractionType.Equip);
        if (gameObject.TryGetComponent(out dragAndDrop))
        {
            dragAndDrop.OnItemDropped += OnDrop;
        }
        itemUI.RegisterEffects(equipmentItem.amplifiers);
        if (dragAndDrop.objectThisAttachedTo.inventory.inventoryTag == Inventory.InventoryTag.Equipment)
        {
            player.playerStats.RegisterAmplifiers(equipmentItem.amplifiers);
        }
    }
    public bool OnDrop(ItemUI itemUI, ItemSlot itemSlot)
    {
        bool equip = itemSlot.inventory.inventoryTag == Inventory.InventoryTag.Equipment
                   && dragAndDrop.objectThisAttachedTo.inventory.inventoryTag != Inventory.InventoryTag.Equipment;

        bool unequip = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment 
                   && dragAndDrop.objectThisAttachedTo.inventory.inventoryTag == Inventory.InventoryTag.Equipment;

        //bool nothing = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment;
        if (itemSlot.expectedItem == ItemSlot.ExpectedItemType.Equippable && equip)
        {
            if (player.equipmentInventory.ItemIsEquippedByName(itemUI.itemStack))
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
        player.playerInventory.EquipItem(itemUI.itemStack);
        player.playerStats.RegisterAmplifiers(equipmentItem.amplifiers);
    }

    public void UnequipItem()
    {
        equipped = false;
        player.equipmentInventory.UnequipItem(itemUI.itemStack);
        player.playerStats.RemoveAmplifiers(equipmentItem.amplifiers);
    }

    private void OnDestroy()
    {
        if (equipped)
        {
            UnequipItem();
        }
    }
}
