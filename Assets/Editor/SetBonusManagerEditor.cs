using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SetBonusManager))]
public class SetBonusManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        SetBonusManager setBonusManager = (SetBonusManager)target;
        if (GUILayout.Button("Find bonuses"))
        {
            setBonusManager.FindBonuses();
        }
    }
}
