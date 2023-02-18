using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemUI))]
[RequireComponent(typeof(DragAndDropItem))]
public class Equipment : MonoBehaviour
{
    public ItemUI itemUI;
    public DragAndDropItem dragAndDrop;
    public EquipmentItem equipmentItem { get; private set; }
    private bool equipped = false;
    void Start()
    {
        equipmentItem = itemUI.inventory.itemManager.GetEquipmentItemBlueprint(itemUI.itemStack);
        itemUI.SetInteractionType(InteractionType.Equip);
        if (gameObject.TryGetComponent(out dragAndDrop))
        {
            dragAndDrop.OnItemDropped += OnDrop;
        }
        RegisterEffects();
    }

    public void RegisterEffects()
    {
        if (equipmentItem.hideDescription)
        {
            string[] desc = new string[equipmentItem.amplifiers.Length];
            for (int i = 0; i < desc.Length; i++)
            {
                desc[i] = "??? ? ?";
            }
            itemUI.RegisterEffects(desc);
        }
        else
        {
            itemUI.RegisterEffects(equipmentItem.amplifiers);
        }
    }
    public bool OnDrop(ItemUI itemUI, ItemSlot itemSlot)
    {
        //return false if the drop can't happen
        //the itemslot was empty, so this is a condition on top of emptyness
        bool equip = itemSlot.inventory.inventoryTag == Inventory.InventoryTag.Equipment
                   && dragAndDrop.slotThisAttachedTo.inventory.inventoryTag != Inventory.InventoryTag.Equipment;

        bool unequip = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment 
                   && dragAndDrop.slotThisAttachedTo.inventory.inventoryTag == Inventory.InventoryTag.Equipment;

        //bool nothing = itemSlot.inventory.inventoryTag != Inventory.InventoryTag.Equipment;
        if (itemSlot.expectedItem == ItemSlot.ExpectedItemType.Equippable && equip)
        {
            bool wasEquipped = itemUI.player.equipmentInventory.ItemIsEquippedByName(itemUI.itemStack);
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
        
    }

    public void UnequipItem()
    {
        equipped = false;
        itemUI.player.equipmentInventory.UnequipItem(itemUI.itemStack);
    }

    private void OnDestroy()
    {
        if (equipped)
        {
            UnequipItem();
        }
    }
}
