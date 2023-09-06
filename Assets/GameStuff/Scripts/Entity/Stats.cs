using System;
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

[Serializable]
public struct OffensiveAtributeData
{
    public OffensiveAttribute attribute;
    public float value;
}

[Serializable]
public struct DefensiveAtributeData
{
    public DefensiveAttribute attribute;
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

    public void CreateAmplifierSystem(EntityData entityData)
    {
        this.entityData = entityData;
        if (entityData == null)
        {
            return;
        }
        amplifierSystem = new AmplifierSystem(entityData.attributeDatas);
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
