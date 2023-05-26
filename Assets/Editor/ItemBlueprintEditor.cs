using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemBlueprint))]

public class ItemBlueprintEditor : Editor
{
    public void GenerateId(ref ItemBlueprint item)
    {
        item.id = System.Guid.NewGuid().ToString();
    }
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        ItemBlueprint item = (ItemBlueprint)target;
        if (item.id == null || item.id == "")
        {
            GenerateId(ref item);
        }
    }
}

[CustomEditor(typeof(OrbBlueprint))]
public class OrbItemEditor : ItemBlueprintEditor
{

}

[CustomEditor(typeof(Potion))]
public class PotionEditor : ItemBlueprintEditor
{

}

