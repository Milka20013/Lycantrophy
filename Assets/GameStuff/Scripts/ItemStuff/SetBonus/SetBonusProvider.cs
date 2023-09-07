using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetBonusProvider
{
    SetBonusManager setBonusManager;

    public SetBonusProvider(SetBonusManager setBonusManager)
    {
        this.setBonusManager = setBonusManager;
    }

    public static OrbBlueprint[] GetEquipmentItems(List<ItemStack> itemStacks)
    {
        OrbBlueprint[] equipmentItems = new OrbBlueprint[itemStacks.Count];
        for (int i = 0; i < equipmentItems.Length; i++)
        {
            equipmentItems[i] = itemStacks[i].itemUI.GetComponent<Orb>().orbBlueprint;
        }
        return equipmentItems;
    }
    public Amplifier[] GetAllSetBonus(OrbBlueprint[] items)
    {
        if (items.Length != 3)
        {
            return null;
        }
        List<Amplifier> amps = new();
        for (int i = 0; i < 7; i++)
        {
            amps.AddRange(GetSetBonus(items, i + 3));
        }
        return amps.ToArray();
    }

    public Amplifier[] GetAllSetBonusEditorInspect(SetBonusBlueprint setBonusBlueprint)
    {
        List<Amplifier> amps = new();
        for (int i = 0; i < 7; i++)
        {
            amps.AddRange(CalculateBonuses(setBonusBlueprint, i + 3));
        }
        return amps.ToArray();
    }
    public Amplifier[] GetSetBonus(OrbBlueprint[] items)
    {

        if (items.Length != 3)
        {
            return null;
        }

        SetTag tag;
        if (!TagCheck(items, out tag))
        {
            return null;
        }

        int level = items.Sum(x => x.tier);
        return CalculateBonuses(setBonusManager.GetBlueprint(tag), level);
    }

    public Amplifier[] GetSetBonus(OrbBlueprint[] items, int level)
    {
        SetTag setTag = items[0].tag;
        return CalculateBonuses(setBonusManager.GetBlueprint(setTag), level);
    }

    public bool TagCheck(OrbBlueprint[] items, out SetTag tag)
    {
        SetTag[] setTags = items.Select(x => x.tag).ToArray();
        tag = null;
        for (int i = 0; i < setTags.Length - 1; i++)
        {
            if (setTags[i] != setTags[i + 1])
            {
                return false;
            }
        }
        tag = setTags[0];
        return true;
    }

    private Amplifier[] CalculateBonuses(SetBonusBlueprint BP, int level)
    {
        if (BP == null)
        {
            return null;
        }
        float[] multipliers = CalculateMultipliers(BP.setBonusVariables, level);
        Amplifier[] amplifiers = Amplifier.CloneArray(BP.setBonusVariables.Select(x => x.amplifier).ToArray());
        for (int i = 0; i < amplifiers.Length; i++)
        {
            amplifiers[i].value *= multipliers[i];
        }
        return amplifiers;

    }

    private float[] CalculateMultipliers(SetBonusVariable[] variables, int level)
    {
        float[] multipliers = new float[variables.Length];
        for (int i = 0; i < multipliers.Length; i++)
        {
            if (variables[i].start <= level)
            {
                multipliers[i] = BonusFunction(variables[i], level) - BonusFunction(variables[i], variables[i].offset - 1);

            }
            else
            {
                multipliers[i] = 0;
            }
        }
        return multipliers;
    }

    private float BonusFunction(SetBonusVariable variable, float level)
    {
        float asd = level + Mathf.Clamp(Mathf.Floor(level / 3) - 1, 0, 10) * variable.weight - 2;
        return level + Mathf.Clamp(Mathf.Floor(level / 3) - 1, 0, 10) * variable.weight - 2;
    }
}
