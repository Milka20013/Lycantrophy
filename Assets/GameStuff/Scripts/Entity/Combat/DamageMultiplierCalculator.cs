using System.Collections.Generic;

public class DamageMultiplierCalculator
{
    private CombatSystem combatSystem;
    public DamageMultiplierCalculator(CombatSystem combatSystem)
    {
        this.combatSystem = combatSystem;
    }

    public float CalculateDamageMultiplier(Stats attackerStats, Stats victimStats)
    {
        var offensiveAttributes = ValidateAttributes(attackerStats.GetOffensiveAttributes());
        HashSet<OffensiveAttribute> offensives = new();
        float result = 1;
        for (int i = 0; i < offensiveAttributes.Count; i++)
        {
            if (offensiveAttributes[i].damaging)
            {
                attackerStats.GetAttributeValue(offensiveAttributes[i], out float multiplier);
                result *= multiplier;
            }
        }
        return result;
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
