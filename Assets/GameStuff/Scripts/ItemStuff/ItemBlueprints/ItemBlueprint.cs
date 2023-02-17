using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName ="ItemBlueprint",menuName ="ItemBlueprint")]
public class ItemBlueprint : ScriptableObject
{
    public string id;
    public string itemName;
    public int stackSize;
    public string basicDescription;
    public GameObject prefab;
    public Sprite sprite;
}
