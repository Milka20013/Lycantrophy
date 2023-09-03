using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SetBonusBlueprint), true)]
public class SetBonusBlueprintEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SetBonusProvider provider = new SetBonusProvider(null);
        if (GUILayout.Button("Show bonuses"))
        {
            var amps = provider.GetAllSetBonusEditorInspect((SetBonusBlueprint)target);
            ClearLog();
            foreach (var item in amps)
            {
                Debug.Log(item);
            }
        }
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
