using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "EntityData/Mob")]
public class MobData : EntityData
{
    public float rawExp;
    public float rawCurrency;
    public List<DroppableItem> droppableItems;
    public List<DroppableItem> generalDrops;
    [Tooltip("Agressive / Passive mob...")]
    public GameObject prefab;
    public GameObject meshPrefab;
    public Vector3 size;
}
