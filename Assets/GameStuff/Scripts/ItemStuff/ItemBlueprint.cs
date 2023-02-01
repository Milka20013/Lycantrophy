using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemBlueprint",menuName ="Item")]
public class ItemBlueprint : ScriptableObject
{
    public int id;
    public string itemName;
    public int stackSize;
    public int prefabId;
}
