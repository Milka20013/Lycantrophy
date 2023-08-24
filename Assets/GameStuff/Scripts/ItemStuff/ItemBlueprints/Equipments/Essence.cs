using System.Collections.Generic;
using UnityEngine;

public class Essence : MonoBehaviour
{
    //to-do : save
    [SerializeField] ItemUI itemUI;

    [HideInInspector] public EssenceBlueprint essenceBlueprint;
    private List<CorePower> corePowers = new();
    [HideInInspector] public List<Amplifier> amplifiers = new();
    private int maxNumberOfCorePowers;

    private void Start()
    {
        essenceBlueprint = itemUI.inventory.itemManager.GetItemBlueprint<EssenceBlueprint>(itemUI.itemStack);
        maxNumberOfCorePowers = essenceBlueprint.maxNumberOfCorePowers;
        itemUI.SetItemType(ItemType.Essence);
        RegisterEffects();
    }

    private void RegisterEffects()
    {
        itemUI.ResetEffects();
        itemUI.RegisterEffects(essenceBlueprint.amplifiers);
        itemUI.RegisterEffects(amplifiers.ToArray());
    }
    public bool RerollPowers()
    {
        if (corePowers.Count <= 0) return false;
        corePowers = essenceBlueprint.RerollCorePowers(corePowers.Count);
        RerollPowerValues();
        return true;
    }

    public bool RerollPower()
    {
        return true;
    }

    public bool RerollPowerValue()
    {
        return true;
    }
    public bool RerollPowerValues()
    {
        if (corePowers.Count <= 0) return false;
        for (int i = 0; i < amplifiers.Count; i++)
        {
            amplifiers[i] = corePowers[i].Reroll();
        }
        RegisterEffects();
        return true;
    }

    public bool AddCorePower()
    {
        if (corePowers.Count >= maxNumberOfCorePowers)
        {
            return false;
        }
        var powersToAdd = essenceBlueprint.AddCorePowers(corePowers.ToArray());
        corePowers.AddRange(powersToAdd);
        for (int i = 0; i < powersToAdd.Count; i++)
        {
            amplifiers.Add(powersToAdd[i].Reroll());
        }
        RegisterEffects();
        return true;
    }
}
