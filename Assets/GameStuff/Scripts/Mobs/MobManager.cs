using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobManager : ScriptableSingleton<MobManager>
{
    private MobBlueprint[] mobBlueprints;
    public AggressiveMob[] aggressiveMobs;

    private void OnEnable()
    {
        mobBlueprints = new MobBlueprint[aggressiveMobs.Length];
        int j = 0;
        for (int i = 0; i < aggressiveMobs.Length; i++)
        {
            mobBlueprints[i] = aggressiveMobs[i];
            j++;
        }
    }

    
}
