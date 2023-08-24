using UnityEngine;

public class CorePowerModifier : MonoBehaviour
{

    [SerializeField] private ItemUI itemUI;
    private CorePowerModifierBlueprint corePowerModifierBp;

    private void Start()
    {
        corePowerModifierBp = itemUI.inventory.itemManager.GetItemBlueprint<CorePowerModifierBlueprint>(itemUI.itemStack);
        itemUI.SetItemType(ItemType.Droppable);
        itemUI.OnDropLogic += OnDropLogic;
    }

    public void OnDropLogic(ItemUI otherItemUI)
    {
        if (otherItemUI.itemStack.item.itemType == ItemType.Essence)
        {
            if (!corePowerModifierBp.ModifyEssence(otherItemUI.gameObject.GetComponent<Essence>()))
            {
                return;
            }
            this.itemUI.itemStack.RemoveItems(1);
            otherItemUI.RefreshItemDescriptionPanel();
        }
    }
}
