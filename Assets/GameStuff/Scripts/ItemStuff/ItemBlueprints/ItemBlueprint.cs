using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemBlueprint",menuName ="ItemBlueprint")]
public class ItemBlueprint : ScriptableObject
{
    public int id;
    public string itemName;
    public int stackSize;
    [Tooltip("The prefab should be in this place (+1) on the ItemSpawner prefabs section")]
    public int prefabId;
}
