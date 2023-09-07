using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "EntityData/Entity")]
public class EntityData : ScriptableObject
{
    public AttributeData[] attributeDatas;
    public AttributeData[] offensiveAtributeDatas;
    public AttributeData[] defensiveAtributeDatas;

}
