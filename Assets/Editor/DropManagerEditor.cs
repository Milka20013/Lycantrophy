using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DropManager))]
public class DropManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        DropManager dropManager = (DropManager)target;
        if (GUILayout.Button("Remove General Drops"))
        {
            dropManager.RemoveGeneralDrops();
        }
        if (GUILayout.Button("Add General Drops"))
        {
            dropManager.AddGeneralDrops();
        }
        if (GUILayout.Button("Refresh MobDatas"))
        {
            dropManager.FindMobDatas();
        }
    }
}
