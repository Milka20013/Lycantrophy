using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class SetBonusDescription : MonoBehaviour
{
    public EquipmentInventory inv;
    public TextMeshProUGUI setName;
    public TextMeshProUGUI[] bonusTexts;

    [SerializeField] private Color highlightColor;
    [SerializeField] private Color ignoreColor;

    public void ShowPanel()
    {
        UpdatePanel();
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePanel()
    {
        string tag = inv.stacksInInventory[0].itemUI.GetComponent<Equipment>().equipmentItem.tag.setName;

        setName.text = tag;

        EquipmentItem[] equipmentItems = SetBonusProvider.GetEquipmentItems(inv.stacksInInventory);

        int level = equipmentItems.Sum(x => x.tier);

        Amplifier[] amps = inv.setBonusProvider.GetAllSetBonus(equipmentItems);
        int ratio = amps.Length / bonusTexts.Length;
        string[] descriptions = new string[amps.Length];

        for (int i = 0; i < amps.Length; i++)
        {
            descriptions[i] = amps[i].Description();
            if (amps[i].value == 0)
            {
                descriptions[i] = "";
            }
        }

        string[] temp = new string[ratio];
        for (int i = 0; i < bonusTexts.Length; i++)
        {
            for (int j = 0; j < ratio; j++)
            {
                temp[j] = descriptions[j + ratio * i];
            }
            bonusTexts[i].text = Join(temp);
            if (i == level - 3)
            {
                bonusTexts[i].color = highlightColor;
            }
            else
            {
                bonusTexts[i].color = ignoreColor;
            }
        }
    }

    private string Join(string[] array)
    {
        string s = "";
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == "")
            {
                continue;
            }
            if (i < array.Length - 1)
            {
                s += array[i] + "\n";
            }
            else
            {
                s += array[i];
            }
        }
        return s;
    }
}
