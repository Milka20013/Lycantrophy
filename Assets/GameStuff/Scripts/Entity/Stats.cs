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
public struct AttributeData<T> where T : Attribute
{
    public AttributeData(AttributeData<OffensiveAttribute> data)
    {
        this.attribute = data.attribute as T;
        this.value = data.value;
    }
    public AttributeData(AttributeData<DefensiveAttribute> data)
    {
        this.attribute = data.attribute as T;
        this.value = data.value;
    }
    public T attribute;
    public float value;

    public static implicit operator AttributeData<T>(AttributeData<OffensiveAttribute> data) => new AttributeData<OffensiveAttribute>(data);
    public static implicit operator AttributeData<T>(AttributeData<DefensiveAttribute> data) => new AttributeData<DefensiveAttribute>(data);
}

public class Stats : MonoBehaviour
{
    public EntityData entityData;

    private AmplifierSystem amplifierSystem;


    public bool save;

    public delegate void ChangeHandler();
    public ChangeHandler OnStatChange;



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

    private void CreateAmplifierSystem(EntityData entityData)
    {
        this.entityData = entityData;
        if (entityData == null)
        {
            return;
        }
        List<AttributeData<Attribute>> attributeDatas = new();
        attributeDatas.AddRange(entityData.attributeDatas);
        foreach (var item in entityData.defensiveAtributeDatas)
        {
            attributeDatas.Add(new AttributeData<Attribute>(item));
        }
        foreach (var item in entityData.offensiveAtributeDatas)
        {
            attributeDatas.Add(new AttributeData<Attribute>(item));
        }
        amplifierSystem = new AmplifierSystem(attributeDatas.ToArray());
    }
    public bool RegisterAmplifiers(Amplifier[] amplifiers)
    {
        bool result = amplifierSystem.RegisterAmplifiers(amplifiers);
        if (result)
        {
            OnStatChange?.Invoke();
        }
        return result;
    }
    public bool RegisterAmplifiers(Amplifier amplifier)
    {
        return RegisterAmplifiers(new Amplifier[] { amplifier });
    }
    public bool UnRegisterAmplifiers(Amplifier[] amplifiers)
    {
        bool result = amplifierSystem.UnregisterAmplifiers(amplifiers);
        if (result)
        {
            OnStatChange?.Invoke();
        }
        return result;
    }
    public bool UnRegisterAmplifiers(Amplifier amplifier)
    {
        return UnRegisterAmplifiers(new Amplifier[] { amplifier });
    }

    public bool GetAttributeValue(Attribute attribute, out float value)
    {
        return amplifierSystem.GetAttributeValue(attribute, out value);
    }

    public OffensiveAttribute[] GetOffensiveAttributes()
    {
        return entityData.offensiveAtributeDatas.Select(x => x.attribute).ToArray();
    }

    public DefensiveAttribute[] GetDefensiveAttributes()
    {
        return entityData.defensiveAtributeDatas.Select(x => x.attribute).ToArray();
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
