using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmplifierSystem
{
    public AttributeData[] attributeDatas;
    public Dictionary<Attribute, float> attributesDict = new Dictionary<Attribute, float>();

    public Dictionary<Attribute, Dictionary<AmplifierType, float>> amplifiersDict = new Dictionary<Attribute, Dictionary<AmplifierType, float>>();

    public AmplifierSystem(AttributeData[] attributeDatas)
    {
        this.attributeDatas = attributeDatas;
        InstantiateSystem();
    }

    public List<Amplifier> everyAmplifier = new List<Amplifier>();

    public void InstantiateSystem()
    {
        attributesDict = FillAttributeDict();
    }

    public Dictionary<Attribute, float> FillAttributeDict() //fill attributes dict with base values
    {
        Dictionary<Attribute, float> dict = new Dictionary<Attribute, float>();
        for (int i = 0; i < attributeDatas.Length; i++)
        {
            dict.Add(attributeDatas[i].attribute, attributeDatas[i].value);
        }
        return dict;
    }

    private Dictionary<Attribute, Dictionary<AmplifierType, float>> FillAmplifiersDict() //fill amps dict with base values (0(6) 1(3))
    {
        Dictionary<Attribute, Dictionary<AmplifierType, float>> dict1 = new Dictionary<Attribute, Dictionary<AmplifierType, float>>();
        for (int i = 0; i < attributeDatas.Length; i++)
        {
            Dictionary<AmplifierType, float> dict2 = new Dictionary<AmplifierType, float>();
            foreach (AmplifierType e2 in Enum.GetValues(typeof(AmplifierType)))
            {
                switch (e2)
                {
                    case AmplifierType.TruePercentage:
                        dict2.Add(e2, 1);
                        break;
                    default:
                        dict2.Add(e2, 0);
                        break;
                }
            }
            dict1.Add(attributeDatas[i].attribute, dict2);
        }
        return dict1;
    }
    public void CalculateAttributeValues() //calculates the raw values for the attributes from the SUMMED UP amplifiers
    {
        attributesDict = FillAttributeDict();
        foreach (KeyValuePair<Attribute, Dictionary<AmplifierType, float>> amps in amplifiersDict)
        {
            foreach (KeyValuePair<AmplifierType, float> core in amps.Value)
            {
                if (AddThis(core.Key))
                {
                    attributesDict[amps.Key] += core.Value;
                }
                else
                {
                    if (core.Value != 0)
                    {
                        attributesDict[amps.Key] *= core.Value;

                    }
                }
            }
            Debug.Log(amps.Key.ToString() + ": " + attributesDict[amps.Key]);
        }
    }
    public void CalculateAmplifierValues() //calculates the raw values for the amplifiersDict from the amplifiers
    {
        amplifiersDict = FillAmplifiersDict();
        float value;
        for (int i = 0; i < everyAmplifier.Count; i++)
        {
            if (AddThis(everyAmplifier[i].amplifierType, true))
            {
                if (everyAmplifier[i].amplifierType == AmplifierType.Percentage)
                {
                    value = 1 + everyAmplifier[i].value / 100;
                }
                else
                {
                    value = everyAmplifier[i].value;
                }
                amplifiersDict[everyAmplifier[i].attribute][everyAmplifier[i].amplifierType] += value;
            }
            else
            {
                amplifiersDict[everyAmplifier[i].attribute][everyAmplifier[i].amplifierType] *= 1 + everyAmplifier[i].value / 100;
            }
        }
        CalculateAttributeValues();
    }
    private bool AddThis(AmplifierType ampType, bool inside = false) //determines if the calculation is adding or multiplying
    {
        if (inside)
        {
            switch (ampType)
            {
                case AmplifierType.Plus:
                    return true;
                case AmplifierType.Percentage:
                    return true;
                case AmplifierType.TruePercentage:
                    return false;
            }
        }
        switch (ampType)
        {
            case AmplifierType.Plus:
                return true;
            default:
                return false;
        }
    }
    public bool RegisterAmplifiers(Amplifier[] amplifiers)
    {
        bool changeHappened = false;
        for (int i = 0; i < amplifiers.Length; i++)
        {
            if (amplifiers[i] == null)
            {
                continue;
            }
            if (Amplifier.IsAmplifierInCollectionPartially(everyAmplifier, amplifiers[i], out int index)) //if the amplifier is already registered, decide what to do
            {
                if (amplifiers[i].key == AmplifierKey.Overriding)
                {
                    everyAmplifier[index] = amplifiers[i];
                    changeHappened = true;
                }
            }
            else
            {
                everyAmplifier.Add(amplifiers[i]); //put in the list if it is not already in it
                changeHappened = true;
            }
        }
        if (changeHappened)
        {
            CalculateAmplifierValues();
        }
        return changeHappened;
    }
    public bool RemoveAmplifiers(Amplifier[] amplifiers)
    {
        bool changeHappened = false;
        if (amplifiers == null)
        {
            return false;
        }
        for (int i = 0; i < amplifiers.Length; i++)
        {
            for (int j = 0; j < everyAmplifier.Count; j++)
            {
                if (amplifiers[i] != null && amplifiers[i].EqualsTo(everyAmplifier[j]))
                {
                    everyAmplifier[j] = null;
                    changeHappened = true;
                    break;
                }
            }
        }
        everyAmplifier.RemoveAll(x => x == null);
        if (changeHappened)
        {
            CalculateAmplifierValues();
        }
        return changeHappened;
    }
    public float GetAttributeValue(Attribute attribute)
    {
        return attributesDict[attribute];
    }
}
