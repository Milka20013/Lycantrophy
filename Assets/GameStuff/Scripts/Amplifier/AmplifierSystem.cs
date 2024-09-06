using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AmplifierSystemData
{
    public Amplifier[] amplifiers;

    public AmplifierSystemData(Amplifier[] amplifiers)
    {
        this.amplifiers = amplifiers;
    }
}

public class AmplifierSystem
{
    public AttributeData<Attribute>[] attributeDatas;
    public Dictionary<Attribute, float> attributesDict = new();

    public Dictionary<Attribute, Dictionary<AmplifierType, float>> amplifiersDict = new();

    public List<Amplifier> everyAmplifier = new();

    public AmplifierSystem(AttributeData<Attribute>[] attributeDatas)
    {
        this.attributeDatas = attributeDatas;
        InstantiateSystem();
    }


    public void InstantiateSystem()
    {
        attributesDict = FillAttributeDict(attributeDatas);
    }

    public static Dictionary<Attribute, float> FillAttributeDict(AttributeData<Attribute>[] attributeDatas) //fill attributes dict with base values
    {
        Dictionary<Attribute, float> dict = new();
        for (int i = 0; i < attributeDatas.Length; i++)
        {
            dict.Add(attributeDatas[i].attribute, attributeDatas[i].value);
        }
        return dict;
    }

    private Dictionary<Attribute, Dictionary<AmplifierType, float>> FillAmplifiersDict() //fill amps dict with base values (0(6) 1(3))
    {
        Dictionary<Attribute, Dictionary<AmplifierType, float>> dict1 = new();
        for (int i = 0; i < attributeDatas.Length; i++)
        {
            Dictionary<AmplifierType, float> dict2 = new();
            foreach (AmplifierType e2 in Enum.GetValues(typeof(AmplifierType)))
            {
                switch (e2)
                {
                    case AmplifierType.TruePercentage:
                        if (attributeDatas[i].attribute.invertedCalculation)
                        {
                            dict2.Add(e2, 0);
                        }
                        else
                        {
                            dict2.Add(e2, 1);
                        }
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
    private void CalculateAttributeValues() //calculates the raw values for the attributes from the SUMMED UP amplifiers
    {
        attributesDict = FillAttributeDict(attributeDatas);
        foreach (KeyValuePair<Attribute, Dictionary<AmplifierType, float>> amps in amplifiersDict)
        {
            attributesDict[amps.Key] = AmplifierValueCalculator.FinalAttributeValue(amps.Value,
                                                                      attributesDict[amps.Key], amps.Key.invertedCalculation);

        }
        LogAttributes();
    }
    private void CalculateAmplifierValues() //calculates the raw values for the amplifiersDict from the amplifiers
    {
        amplifiersDict = FillAmplifiersDict();
        for (int i = 0; i < everyAmplifier.Count; i++)
        {
            AmplifierValueCalculator.SummarizeAmplifierValues(ref amplifiersDict, everyAmplifier[i]);
        }
        CalculateAttributeValues();
    }

    private void LogAttributes()
    {
        foreach (KeyValuePair<Attribute, Dictionary<AmplifierType, float>> amps in amplifiersDict)
        {
            Debug.Log(amps.Key.ToString() + ": " + attributesDict[amps.Key]);
        }
    }

    private void LogAmplifiers()
    {
        Debug.Log("-----");
        foreach (var item in everyAmplifier)
        {
            Debug.Log(item);
        }
    }
    public bool RegisterAmplifiers(Amplifier[] amplifiers)
    {
        if (amplifiers == null)
        {
            return false;
        }
        bool changeHappened = false;
        for (int i = 0; i < amplifiers.Length; i++)
        {
            if (amplifiers[i] == null)
            {
                continue;
            }
            if (Amplifier.IsAmplifierInCollectionExcludingValue(everyAmplifier, amplifiers[i], out int index)) //if the amplifier is already registered, decide what to do
            {
                switch (amplifiers[i].key)
                {
                    case AmplifierKey.Max:
                        if (everyAmplifier[index].value < amplifiers[i].value)
                        {
                            everyAmplifier[index] = amplifiers[i];
                            changeHappened = true;
                        }
                        break;
                    case AmplifierKey.Min:
                        if (everyAmplifier[index].value > amplifiers[i].value)
                        {
                            everyAmplifier[index] = amplifiers[i];
                            changeHappened = true;
                        }
                        break;
                    case AmplifierKey.Override:
                        everyAmplifier[index] = amplifiers[i];
                        changeHappened = true;
                        break;
                    default:
                        break;
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

    public bool UnregisterAmplifiers(Amplifier[] amplifiers)
    {
        bool changeHappened = false;
        if (amplifiers == null)
        {
            return false;
        }
        for (int i = 0; i < amplifiers.Length; i++)
        {
            if (amplifiers[i] == null)
            {
                continue;
            }
            for (int j = 0; j < everyAmplifier.Count; j++)
            {
                if (amplifiers[i] != null && everyAmplifier[j] != null && amplifiers[i].EqualsTo(everyAmplifier[j]))
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
    public bool GetAttributeValue(Attribute attribute, out float value)
    {
        if (attributesDict.TryGetValue(attribute, out value))
        {
            return true;
        }
        return false;
    }
}
