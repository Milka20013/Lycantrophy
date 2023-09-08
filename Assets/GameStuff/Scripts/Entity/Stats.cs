using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*public enum Attribute
{
    None,
    Damage,
    MaxHealth,
    MoveSpeed,
    AttackSpeed,
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Type6
}*/

[Serializable]
public struct AttributeData
{
    public Attribute attribute;
    public float value;
}

public class Stats : MonoBehaviour
{
    public EntityData entityData;

    private AmplifierSystem amplifierSystem;


    public bool save;

    public delegate void ChangeHandler();
    public event ChangeHandler OnStatChange;


    private void Awake()
    {
        if (amplifierSystem == null)
        {
            CreateAmplifierSystem(entityData);
        }
    }

    private void Start()
    {
        OnStatChange?.Invoke();
    }

    public void CreateAmplifierSystem(EntityData entityData)
    {
        this.entityData = entityData;
        if (entityData == null)
        {
            return;
        }
        List<AttributeData> attributeDatas = new();
        attributeDatas.AddRange(entityData.attributeDatas);
        attributeDatas.AddRange(entityData.defensiveAtributeDatas);
        attributeDatas.AddRange(entityData.offensiveAtributeDatas);
        amplifierSystem = new AmplifierSystem(attributeDatas.ToArray());
    }
    public void RegisterAmplifiers(Amplifier[] amplifiers)
    {
        if (amplifierSystem.RegisterAmplifiers(amplifiers))
        {
            OnStatChange?.Invoke();
        }
    }

    public void UnRegisterAmplifiers(Amplifier[] amplifiers)
    {
        if (amplifierSystem.UnregisterAmplifiers(amplifiers))
        {
            OnStatChange?.Invoke();
        }
    }


    public bool GetAttributeValue(Attribute attribute, out float value)
    {
        return amplifierSystem.GetAttributeValue(attribute, out value);
    }

    public OffensiveAttribute[] GetOffensiveAttributes()
    {
        return entityData.offensiveAtributeDatas.Select(x => x.attribute as OffensiveAttribute).ToArray();
    }

    public DefensiveAttribute[] GetDefensiveAttributes()
    {
        return entityData.defensiveAtributeDatas.Select(x => x.attribute as DefensiveAttribute).ToArray();
    }

    /*public void Save(ref GameData data)
    {
        if (!save)
        {
            return;
        }
        data.amplifierSystemData = new AmplifierSystemData(amplifierSystem.everyAmplifier.ToArray());
    }

    public void Load(GameData data)
    {
        if (!save)
        {
            return;
        }
        CreateAmplifierSystem(entityData);
        amplifierSystem.RegisterAmplifiers(data.amplifierSystemData.amplifiers);
    }*/
}
