using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "ItemBlueprint/Consumable/Potion")]
public class Potion : ConsumableBlueprint
{
    public override void ConsumeItem(HealthSystem healthSystem)
    {
        healthSystem.InstantHeal(consumeValue);
    }
}
