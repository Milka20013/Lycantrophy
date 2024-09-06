using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum AmplifierType { Plus, Percentage, TruePercentage }
public enum AmplifierKey { Max, Min, Override }
[Serializable]
public class Amplifier
{
    public string tag;
    public AmplifierType amplifierType;
    public Attribute attribute;
    public float value;
    public AmplifierKey key;

    public Amplifier(string tag, AmplifierType amplifierType, Attribute attribute, float value, AmplifierKey key)
    {
        this.tag = tag;
        this.amplifierType = amplifierType;
        this.attribute = attribute;
        this.value = value;
        this.key = key;
    }
    public Amplifier(string tag, AmplifierType amplifierType, Attribute attribute, float value)
    {
        this.tag = tag;
        this.amplifierType = amplifierType;
        this.attribute = attribute;
        this.value = value;
        key = AmplifierKey.Max;
    }
    public Amplifier(Amplifier other)
    {
        this.tag = other.tag;
        this.amplifierType = other.amplifierType;
        this.attribute = other.attribute;
        this.value = other.value;
        key = other.key;
    }
    public override string ToString()
    {
        return tag + " " + amplifierType.ToString() + " " + attribute.ToString() + " " + value.ToString() + " " + key.ToString();
    }
    public string Description()
    {
        string desc = "";

        desc += attribute.ToString();

        string valueStr = Mathf.Abs(value).ToString();

        desc += amplifierType == AmplifierType.TruePercentage ? " * " + valueStr : value >= 0 ? " + " + valueStr : " - " + valueStr;
        desc += amplifierType == AmplifierType.Plus ? "" : "%";

        return desc;
    }
    public static bool IsAmplifierInCollectionExcludingValue(IEnumerable<Amplifier> amplifiers, Amplifier amplifier, out int index)
    {
        //it does pseude check, stacking objects return false although it is in the collection
        // to-do clean this up
        int length = amplifiers.Count();
        for (int i = 0; i < length; i++)
        {
            if (amplifiers.ElementAt(i).EqualsToExcludingValue(amplifier))
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }
    public static Amplifier[] CloneArray(Amplifier[] amplifiers)
    {
        if (amplifiers == null)
        {
            return null;
        }
        Amplifier[] amps = new Amplifier[amplifiers.Length];
        for (int i = 0; i < amplifiers.Length; i++)
        {
            if (amplifiers[i] == null)
            {
                return null;
            }
            amps[i] = new Amplifier(amplifiers[i]);
        }
        return amps;
    }
    public bool EqualsTo(Amplifier other)
    {
        //check if every value is equal
        bool tagB = tag == other.tag;
        bool valueB = value == other.value;
        bool attributeB = attribute == other.attribute;
        bool ampTypeB = amplifierType == other.amplifierType;
        bool keyB = key == other.key;
        bool everyB = tagB && attributeB && ampTypeB && valueB && keyB;
        if (everyB)
        {
            return true;
        }
        return false;
    }
    public bool EqualsToExcludingValue(Amplifier other)
    {
        //check if every value is equal
        bool tagB = tag == other.tag;
        bool attributeB = attribute == other.attribute;
        bool ampTypeB = amplifierType == other.amplifierType;
        bool keyB = key == other.key;
        bool everyB = tagB && attributeB && ampTypeB && keyB;
        if (everyB)
        {
            return true;
        }
        return false;
    }
}
