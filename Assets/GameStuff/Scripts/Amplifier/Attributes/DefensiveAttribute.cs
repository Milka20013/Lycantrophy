using UnityEngine;
[CreateAssetMenu(menuName = "Combat/Attribute/Defensive")]
public class DefensiveAttribute : CombatAttribute
{
    public OffensiveAttribute[] counterAttributes;
}
