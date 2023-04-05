using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : Inventory
{
    [SerializeField] private SetBonusManager setBonusManager;
    [HideInInspector] public SetBonusProvider setBonusProvider;
    [HideInInspector] public Amplifier[] setbonus;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        setBonusProvider = new SetBonusProvider(setBonusManager);
    }

    public void UnequipItem(ItemStack itemStack)
    {
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
        UnRegisterAmplifiers(itemStack.itemUI.GetComponent<Equipment>().equipmentItem.amplifiers);
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            var amps = stacksInInventory[i].itemUI.GetComponent<Equipment>().equipmentItem.amplifiers;
            player.playerStats.RegisterAmplifiers(amps);
        }

        UnRegisterAmplifiers(setbonus);
        setbonus = null;
    }

    public void EquipItem(ItemStack itemStack)
    {
        //the equippable item is not registered yet in stacksInInventory !!
        RegisterAmplifiers(itemStack.itemUI.GetComponent<Equipment>().equipmentItem.amplifiers);

        List<ItemStack> stacks = new();
        stacks.AddRange(stacksInInventory);
        stacks.Add(itemStack);

        setbonus = setBonusProvider.GetSetBonus(SetBonusProvider.GetEquipmentItems(stacks));
        RegisterAmplifiers(setbonus);
    }

    private void RegisterAmplifiers(Amplifier[] amplifiers)
    {
        player.playerStats.RegisterAmplifiers(amplifiers);
    }

    private void UnRegisterAmplifiers(Amplifier[] amplifiers)
    {
        player.playerStats.UnRegisterAmplifiers(amplifiers);
    }

    public bool ItemIsEquippedByName(ItemStack itemStack)
    {
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            if (itemStack.item.itemName == stacksInInventory[i].item.itemName)
            {
                return true;
            }
        }
        return false;
    }

    public bool ItemIsEquippedByRef(ItemStack itemStack)
    {
        return stacksInInventory.Contains(itemStack);
    }
}
