using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmplifierSystem
{
    public float[] baseValues;
    public Dictionary<Attribute, float> attributesDict = new Dictionary<Attribute, float>();

    public Dictionary<Attribute, Dictionary<AmplifierType, float>> amplifiersDict = new Dictionary<Attribute, Dictionary<AmplifierType, float>>();

    public AmplifierSystem(float[] baseValues)
    {
        this.baseValues = baseValues;
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
        foreach (Attribute item in Enum.GetValues(typeof(Attribute)))
        {
            dict.Add(item, baseValues[(int)item]);
        }
        return dict;
    }

    private Dictionary<Attribute, Dictionary<AmplifierType, float>> FillAmplifiersDict() //fill amps dict with base values (0(6) 1(3))
    {
        Dictionary<Attribute, Dictionary<AmplifierType, float>> dict1 = new Dictionary<Attribute, Dictionary<AmplifierType, float>>();
        foreach (Attribute e1 in Enum.GetValues(typeof(Attribute)))
        {
            Dictionary<AmplifierType, float> dict2 = new Dictionary<AmplifierType, float>();
            foreach (AmplifierType e2 in Enum.GetValues(typeof(AmplifierType)))
            {
                switch (e2)
                {
                    case AmplifierType.truePercentage:
                        dict2.Add(e2, 1);
                        break;
                    default:
                        dict2.Add(e2, 0);
                        break;
                }
            }
            dict1.Add(e1, dict2);
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
                if (everyAmplifier[i].amplifierType == AmplifierType.percentage)
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
                case AmplifierType.plus:
                    return true;
                case AmplifierType.percentage:
                    return true;
                case AmplifierType.truePercentage:
                    return false;
            }
        }
        switch (ampType)
        {
            case AmplifierType.plus:
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
                if (amplifiers[i].key == AmplifierKey.overriding)
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
