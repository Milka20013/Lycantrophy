using UnityEngine;
[CreateAssetMenu(fileName = "BoostingPotion", menuName = "ItemBlueprint/Consumable/RestoringPotion")]
public class RestoringPotion : ConsumableBlueprint
{
    public float restoreValue;
    public override void ConsumeItem(Player player)
    {
        player.GetComponent<HealthSystem>().InstantHeal(restoreValue);
    }

    public override string[] FullDescription()
    {
        if (effectDescription.Length > 0)
        {
            return effectDescription;
        }
        return new string[] { "Instantly heals: " + restoreValue };
    }
}
