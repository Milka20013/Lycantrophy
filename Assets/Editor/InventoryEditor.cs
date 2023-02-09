using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    public void GenerateGUID(ref Inventory inv)
    {
        inv.id = System.Guid.NewGuid().ToString();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Inventory inventroy = (Inventory)target;
        if (GUILayout.Button("Generate ID"))
        {
            GenerateGUID(ref inventroy);
        }
    }
}
