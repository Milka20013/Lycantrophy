using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        ItemManager itemManager = (ItemManager)target;
        if (GUILayout.Button("Refresh Items"))
        {
            itemManager.RefreshData();
        }
        if (GUILayout.Button("Clear Items"))
        {
            itemManager.ClearData();
        }
    }
}
