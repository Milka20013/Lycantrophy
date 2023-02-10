using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum MobType
{
    Aggressive, Neutral, Passive
}

[CreateAssetMenu(fileName ="MobManager",menuName ="Manager/Mob")]
public class MobManager : ScriptableSingleton<MobManager>
{
    public GameObject[] prefabs; //has to be in MobType order

    public GameObject GetMobPrefab(MobData mob)
    {
        return prefabs[(int)mob.mobType];
    }

}
