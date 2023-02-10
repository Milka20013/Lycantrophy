using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Entity",menuName ="EntityData/Entity")]
public class EntityData : ScriptableObject
{
    public AttributeData[] attributeDatas;
}
