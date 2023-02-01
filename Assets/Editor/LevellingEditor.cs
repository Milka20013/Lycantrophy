using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Levelling))]
public class LevellingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Levelling levelling = (Levelling)target;
        if (levelling.levelTexts.Length == 0 || levelling.levelTexts[0] == null)
        {
            EditorGUILayout.HelpBox("1 level text with a world space canvas is mandatory", MessageType.Error);
        }
        if (levelling.maxLevel != levelling.milestones.Length)
        {
            EditorGUILayout.HelpBox("Milestones should be as big in size as the maxlevel", MessageType.Warning);
        }
    }
}
