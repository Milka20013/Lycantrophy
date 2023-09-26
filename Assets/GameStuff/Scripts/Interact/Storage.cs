using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Storage : Interactable
{
    [HideInInspector] public Inventory inventory;
    public override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory>();
    }
    public override void Interact(Player player)
    {
        base.Interact(player);
        inventory.OnOpenInventory();
    }
    public override void HideIndicator()
    {
        base.HideIndicator();
        inventory.CloseMenu();
    }
}
