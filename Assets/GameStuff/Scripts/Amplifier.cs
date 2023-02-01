using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AmplifierType { plus, percentage, truePercentage }
public enum AmplifierKey { none, stacking, overriding, extending }
[Serializable]
public class Amplifier
{
    public string tag;
    public AmplifierType amplifierType;
    public Attribute attribute;
    public float value;
    public AmplifierKey key;

    public Amplifier(string tag,AmplifierType amplifierType, Attribute attribute, float value, AmplifierKey key)
    {
        this.tag = tag;
        this.amplifierType = amplifierType;
        this.attribute = attribute;
        this.value = value;
        this.key = key;
    }
    public Amplifier(string tag,AmplifierType amplifierType, Attribute attribute, float value)
    {
        this.tag = tag;
        this.amplifierType = amplifierType;
        this.attribute = attribute;
        this.value = value;
        key = AmplifierKey.none;
    }
    public override string ToString()
    {
        return tag + " " + amplifierType.ToString() + " " + attribute.ToString() + " " + value.ToString() + " " + key.ToString();
    }
    public string Description()
    {
        string desc = "";
        desc += attribute.ToString();
        desc += amplifierType == AmplifierType.plus ? " + " + value.ToString() : " * " + value.ToString();
        return desc;
    }
    public static bool IsAmplifierInCollectionPartially(List<Amplifier> amplifiers, Amplifier amplifier, out int index)
    {
        //it does pseude check, stacking objects return false although it is in the collection
        if (amplifier.key == AmplifierKey.stacking)
        {
            index = -1;
            return false;
        }
        for (int i = 0; i < amplifiers.Count; i++)
        {
            if (amplifier.PartiallyEqualsTo(amplifiers[i]))
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
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
    public bool PartiallyEqualsTo(Amplifier other)
    {
        //check if the tag, the attribute and the amptype is equal
        bool tagB = tag == other.tag;
        bool attributeB = attribute == other.attribute;
        bool ampTypeB = amplifierType == other.amplifierType;
        bool everyB = tagB && attributeB && ampTypeB;
        if (everyB)
        {
            return true;
        }
        return false;
    }
}
