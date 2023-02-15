using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MobData",menuName ="EntityData/Mob")]
public class MobData : EntityData
{
    public float rawExp;
    public DroppableItem[] droppableItems;
    public GameObject prefab;
}
