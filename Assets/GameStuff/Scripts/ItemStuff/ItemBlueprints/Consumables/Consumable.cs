using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class Consumable : MonoBehaviour, IPointerClickHandler
{
    public ItemUI itemUI;

    public ConsumableItem consumableItem;

    private void Start()
    {
        consumableItem = itemUI.inventory.itemManager.GetConsumableItemBlueprint(itemUI.itemStack);
        itemUI.SetInteractionType(InteractionType.Consume);
        itemUI.RegisterEffects(Description());
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemUI.itemStack.state == ItemStack.StackState.Dead)
            {
                return;
            }
            ExecuteEffect();
            itemUI.itemStack.RemoveItems(1);
        }
    }
    public void ExecuteEffect()
    {
        consumableItem.ConsumeItem(itemUI.player.GetComponent<HealthSystem>());
    }

    public string Description()
    {
        return consumableItem.FullDescription();
    }
}
