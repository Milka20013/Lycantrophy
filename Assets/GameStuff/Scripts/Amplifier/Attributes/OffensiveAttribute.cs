using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attribute/Offensive")]
public class OffensiveAttribute : CombatAttribute
{
    [Tooltip("Multiplies the value of the amplifier")]
    public float efficiency;
    public bool damaging;
}
