using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MobData",menuName ="EntityData/Mob")]
public class MobData : EntityData
{
    public float rawExp;
    public List<DroppableItem> droppableItems;
    public List<DroppableItem> generalDrops;
    public GameObject prefab;
    public Mesh mesh;
    public Vector3 size;
}
