using Lycanthropy.Helper;
using System;
using TMPro;
using UnityEngine;
public class StatMenu : MonoBehaviour, IMenu
{
    public Stats playerStats;
    public Levelling levelling;
    [SerializeField] GameObject panel;
    private int statPoints;

    public TextMeshProUGUI statPointsText;

    [SerializeField] private Amplifier[] amplifiers;
    private void Awake()
    {
        levelling.OnLevelUp += OnLevelUp; //subscribe to the method list
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
        for (int i = 0; i < amplifiers.Length; i++)
        {
            if (amplifiers[i].attribute == statSelector.attributeType)
            {
                amplifiers[i].value += statSelector.value;
                statSelector.textToUpdate.text = Math.Round(amplifiers[i].value, 1).ToString();
            }
        }
        statPoints -= 1;
        playerStats.RegisterAmplifiers(amplifiers);
        UpdateStatPoints();
    }

    public void UpdateStatPoints()
    {
        statPointsText.text = "Stat Points: " + statPoints;
    }

    public void OnTriggerMenu()
    {
        if (panel.activeSelf)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void CloseMenu()
    {
        panel.SetActive(false);
    }

    public void OpenMenu()
    {
        panel.SetActive(true);
    }

    public bool IsOpen()
    {
        return panel.activeSelf;
    }
}
