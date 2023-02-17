using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/DropManager")]
public class DropManager : ScriptableSingleton<DropManager>
{
    public MobData[] mobDatas;
    public DroppableItem[] generalDrops;

    private void Awake()
    {
        FindMobDatas();
    }

    public void FindMobDatas()
    {
        mobDatas = Resources.LoadAll<MobData>("MobDatas");
    }
    public void RemoveGeneralDrops()
    {
        for (int i = 0; i < mobDatas.Length; i++)
        {
            mobDatas[i].generalDrops.Clear();
        }
    }
    public void AddGeneralDrops()
    {
        for (int i = 0; i < mobDatas.Length; i++)
        {
            for (int j = 0; j < generalDrops.Length; j++)
            {
                if (!mobDatas[i].generalDrops.Contains(generalDrops[j]))
                {
                    mobDatas[i].generalDrops.Add(generalDrops[j]);
                }
               
            }
        }
    }
}
