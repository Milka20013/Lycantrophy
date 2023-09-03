using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "ItemBlueprint/Consumable/Potion")]
public class Potion : ConsumableBlueprint
{
    public float consumeValue;
    public override void ConsumeItem(Player player)
    {
        var healthSystem = player.GetComponent<HealthSystem>();
        healthSystem.InstantHeal(consumeValue);
    }

    public override string FullDescription()
    {
        return base.FullDescription() + " " + consumeValue + " health";
    }
}
