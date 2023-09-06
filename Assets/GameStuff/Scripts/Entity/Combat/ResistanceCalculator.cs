using System.Collections.Generic;

public class ResistanceCalculator
{
    CombatSystem combatSystem;
    public ResistanceCalculator(CombatSystem combatSystem)
    {
        this.combatSystem = combatSystem;
    }
    public float SummedResistance(Stats attackerStats, Stats victimStats)
    {
        var offensiveAttributes = ValidateAttributes(attackerStats.GetOffensiveAttributes());
        HashSet<OffensiveAttribute> offensives = new();
        for (int i = 0; i < offensiveAttributes.Count; i++)
        {
            offensives.Add(offensiveAttributes[i]);
        }
        var defensiveAttributes = ValidateAttributes(victimStats.GetDefensiveAttributes());
        float result = 1;
        float tmpResistance = 0;
        for (int i = 0; i < defensiveAttributes.Count; i++)
        {
            if (!victimStats.GetAttributeValue(defensiveAttributes[i], out tmpResistance))
            {
                continue;
            }
            for (int j = 0; j < defensiveAttributes[i].counterAttributes.Length; j++)
            {
                if (offensives.TryGetValue(defensiveAttributes[i].counterAttributes[j], out OffensiveAttribute offAttribute))
                {
                    attackerStats.GetAttributeValue(offAttribute, out float penetration);
                    tmpResistance = ResistanceCalculation(tmpResistance, penetration, offAttribute.efficiency);
                }
            }
            result *= (1 - tmpResistance);
        }
        return 1 - result;
    }

    private float ResistanceCalculation(float resistance, float penetration, float efficiency)
    {
        return resistance * (1 - penetration * efficiency);
    }

    private List<T> ValidateAttributes<T>(T[] attributes) where T : CombatAttribute
    {
        List<T> result = new();
        for (int i = 0; i < attributes.Length; i++)
        {
            if (combatSystem.ValidAttributeInContext(attributes[i]))
            {
                result.Add(attributes[i]);
            }
        }
        return result;
    }
}
