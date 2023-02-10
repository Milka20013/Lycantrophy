using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

//if changed, change FillBaseValues as well
public enum Attribute { 
    Damage, 
    MaxHealth, 
    MoveSpeed, 
    AttackSpeed 
}

[Serializable]
public struct AttributeData
{
    public Attribute attribute;
    public float value;
}
public class Stats : MonoBehaviour
{
    public AttributeData[] attributeDatas;

    private AmplifierSystem amplifierSystem;



    public delegate void ChangeHandler(); //creating a delegate, so that other scripts can subscribe to it
    public event ChangeHandler OnStatChange;

    private void Awake()
    {
        amplifierSystem = new AmplifierSystem(attributeDatas);
    }


    public void RegisterAmplifiers(Amplifier[] amplifiers)
    {
        if (amplifierSystem.RegisterAmplifiers(amplifiers)) //if change happened to the amps, this returns true
        {
            //invoke all the methods registered
            OnStatChange?.Invoke();
        }
    }

    public void UnRegisterAmplifiers(Amplifier[] amplifiers)
    {
        if (amplifierSystem.RemoveAmplifiers(amplifiers)) //if change happened to the amps, this returns true
        {
            if (OnStatChange != null) //so we call the methods that listens to this event
            {
                OnStatChange();
            }
        }
    }


    public float GetAttributeValue(Attribute attribute)
    {
        return amplifierSystem.GetAttributeValue(attribute);
    }
}
