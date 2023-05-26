using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Storage : Interactable
{
    private Inventory inventory;
    public override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory>();
    }
    public override void Interact()
    {
        base.Interact();
        inventory.OnOpenInventory();
    }
    public override void HideIndicator()
    {
        base.HideIndicator();
        inventory.CloseInventory();
    }
}
