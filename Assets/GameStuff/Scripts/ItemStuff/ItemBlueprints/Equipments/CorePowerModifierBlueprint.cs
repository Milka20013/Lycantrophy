using UnityEngine;


[CreateAssetMenu(fileName = "CorePowerModifierBP", menuName = "CorePower/CorePowerModifier")]
public class CorePowerModifierBlueprint : ItemBlueprint
{
    public enum ModificationType
    {
        Add,
        RerollValue,
        RerollValues,
        RerollPower,
        RerollPowers
    }
    public ModificationType modificationType;

    public bool ModifyEssence(Essence essence)
    {
        switch (modificationType)
        {
            case ModificationType.Add:
                return essence.AddCorePower();
            case ModificationType.RerollValue:
                return essence.RerollPowerValue();
            case ModificationType.RerollValues:
                return essence.RerollPowerValues();
            case ModificationType.RerollPower:
                return essence.RerollPower();
            case ModificationType.RerollPowers:
                return essence.RerollPowers();
            default:
                return true;
        }
    }
}
