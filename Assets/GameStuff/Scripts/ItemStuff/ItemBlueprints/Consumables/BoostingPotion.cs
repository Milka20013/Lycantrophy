using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostingPotion", menuName = "ItemBlueprint/Consumable/BoostingPotion")]
public class BoostingPotion : ConsumableBlueprint
{
    public TimedAmplifier[] boostingAmplifiers;

    public override void ConsumeItem(Player player)
    {
        TempAmplifierManager.instance.RegisterAmplifiers(boostingAmplifiers, player.GetComponent<Stats>());
    }

    public override string[] FullDescription()
    {
        if (effectDescription.Length > 0)
        {
            return effectDescription;
        }
        return boostingAmplifiers.Select(x => x.Description()).ToArray();
    }
}
