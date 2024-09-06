using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "EntityData/Entity")]
public class EntityData : ScriptableObject
{
    public AttributeData<Attribute>[] attributeDatas;
    public AttributeData<OffensiveAttribute>[] offensiveAtributeDatas;
    public AttributeData<DefensiveAttribute>[] defensiveAtributeDatas;

}
