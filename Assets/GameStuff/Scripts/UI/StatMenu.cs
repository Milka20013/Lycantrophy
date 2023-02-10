using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum StatType
{
    Strength, Vitality, Agility
}
public class StatMenu : MonoBehaviour
{
    public Stats playerStats;
    public Levelling levelling;
    private int statPoints;

    public TextMeshProUGUI statPointsText;
    public TextMeshProUGUI[] statLevelTexts;

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
                case StatType.Vitality:
                    amps[i] = new Amplifier("stat",AmplifierType.Plus, Attribute.MaxHealth, stat.Value * 5, AmplifierKey.Overriding);
                    break;
                case StatType.Strength:
                    amps[i] = new Amplifier("stat", AmplifierType.Plus, Attribute.Damage, stat.Value * 2, AmplifierKey.None);
                    break;
                case StatType.Agility:
                    amps[i] = new Amplifier("stat", AmplifierType.Plus, Attribute.MoveSpeed, stat.Value, AmplifierKey.Overriding);
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
        UpdateStatPoints();
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
        statLevelTexts[(int)statSelector.statType].text = stats[statSelector.statType].ToString();
        UpdateStatPoints();
    }

    public void UpdateStatPoints()
    {
        statPointsText.text = "Stat Points: " + statPoints;
    }

    
    private void UpdateAmplifiers() //update the amplifiers according to the stats
    {
        int i = 0;
        foreach (KeyValuePair<StatType, int> stat in stats)
        {
            switch (stat.Key)
            {
                case StatType.Vitality:
                    amplifiers[i].value = stat.Value * 5;
                    break;
                case StatType.Strength:
                    amplifiers[i].value = stat.Value * 2;
                    break;
                case StatType.Agility:
                    amplifiers[i].value = stat.Value;
                    break;
                default:
                    break;
            }
            i++;
        }
    }
}
