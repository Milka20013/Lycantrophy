using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatType
{
    vitality, strength, agility
}
public class StatMenu : MonoBehaviour
{
    public PlayerStats playerStats;
    public Levelling levelling;
    public GameObject statMenuUI;
    private int statPoints;

    private Dictionary<StatType, int> stats = new Dictionary<StatType, int>();
    private Amplifier[] amplifiers;
    
    private void Awake()
    {
        levelling.OnLevelUp += OnLevelUp; //subscribe to the method list
        stats = FillUpStatDictionary(); //instantiating the stats
        amplifiers = ConvertStatsToAmplifiers(); //insstantiating the amps
    }
    private Dictionary<StatType, int> FillUpStatDictionary() //creates the dictionary of the stats (should only use once)
    {
        Dictionary<StatType, int> dict = new Dictionary<StatType, int>();
        foreach (StatType item in Enum.GetValues(typeof(StatType)))
        {
            dict.Add(item, 0);
        }
        return dict;
    }

    private Amplifier[] ConvertStatsToAmplifiers() //creates the amplifiers from the dictionary (should only use once)
    {
        Amplifier[] amps = new Amplifier[stats.Count];
        int i = 0;
        foreach (KeyValuePair<StatType, int> stat in stats)
        {
            switch (stat.Key)
            {
                case StatType.vitality:
                    amps[i] = new Amplifier("stat",AmplifierType.plus, Attribute.MaxHealth, stat.Value * 5, AmplifierKey.overriding);
                    break;
                case StatType.strength:
                    amps[i] = new Amplifier("stat", AmplifierType.plus, Attribute.Damage, stat.Value * 2, AmplifierKey.none);
                    break;
                case StatType.agility:
                    amps[i] = new Amplifier("stat", AmplifierType.plus, Attribute.Speed, stat.Value, AmplifierKey.overriding);
                    break;
                default:
                    break;
            }
            i++;
        }
        return amps;
    }
    public void OnLevelUp(int currentLevel) //method on levelling script
    {
        statPoints += 2;
    }
    public void OnOpenStatMenu() //press c
    {
        statMenuUI.SetActive(!statMenuUI.activeSelf);
        Cursor.lockState = CursorLockMode.None;
    }
    public void IncreaseStat(StatSelector statSelector)
    {
        if (statPoints <= 0)
        {
            return;
        }
        stats[statSelector.statType] += 1;
        statPoints -= 1;
        UpdateAmplifiers();
        playerStats.RegisterAmplifiers(amplifiers);
    }

    
    private void UpdateAmplifiers() //update the amplifiers according to the stats
    {
        int i = 0;
        foreach (KeyValuePair<StatType, int> stat in stats)
        {
            switch (stat.Key)
            {
                case StatType.vitality:
                    amplifiers[i].value = stat.Value * 5;
                    break;
                case StatType.strength:
                    amplifiers[i].value = stat.Value * 2;
                    break;
                case StatType.agility:
                    amplifiers[i].value = stat.Value;
                    break;
                default:
                    break;
            }
            i++;
        }
    }
}
