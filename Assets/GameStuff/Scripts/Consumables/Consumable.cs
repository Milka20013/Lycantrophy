using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Consumable : MonoBehaviour, IPointerClickHandler
{
    private Player player;
    public ItemUI itemUI;

    public ConsumableItem consumableItem;

    private void Start()
    {
        player = itemUI.player;
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
        consumableItem.ConsumeItem(player);
    }

    public string Description()
    {
        return consumableItem.FullDescription();
    }
}
