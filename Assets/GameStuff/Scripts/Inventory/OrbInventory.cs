using UnityEngine;

public class OrbInventory : Inventory
{
    [SerializeField] private SetBonusManager setBonusManager;
    [HideInInspector] public SetBonusProvider setBonusProvider;
    [HideInInspector] public Amplifier[] setbonus;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        setBonusProvider = new SetBonusProvider(setBonusManager);
    }

    public override void RegisterItemStack(ItemStack itemStack)
    {
        base.RegisterItemStack(itemStack);
        EquipItem(itemStack);
    }

    public override void UnRegisterItemStack(ItemStack itemStack)
    {
        base.UnRegisterItemStack(itemStack);
        UnequipItem(itemStack);
    }

    public void UnequipItem(ItemStack itemStack)
    {
        UnRegisterAmplifiers(itemStack.itemUI.GetComponent<Orb>().orbBlueprint.amplifiers);

        //the item that is enuqipped might have the same amps as the others, so we recalculate
        for (int i = 0; i < stacksInInventory.Count; i++)
        {
            var amps = stacksInInventory[i].itemUI.GetComponent<Orb>().orbBlueprint.amplifiers;
            player.playerStats.RegisterAmplifiers(amps);
        }

        UnRegisterAmplifiers(setbonus);
        setbonus = null;
    }

    public void EquipItem(ItemStack itemStack)
    {
        //the equippable item is already registered in stacksInInventory
        RegisterAmplifiers(itemStack.itemUI.GetComponent<Orb>().orbBlueprint.amplifiers);

        setbonus = setBonusProvider.GetSetBonus(SetBonusProvider.GetEquipmentItems(stacksInInventory));
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

    public override T GetOwner<T>() where T : class
    {
        if (typeof(T) == typeof(Player))
        {
            return player as T;
        }
        return null;
    }
}
