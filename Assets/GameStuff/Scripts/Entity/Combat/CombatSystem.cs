using System.Collections.Generic;

public class CombatSystem
{
    private Stats ownStats;
    public HashSet<CombatCondition> conditions = new();

    private ResistanceCalculator resCalc;

    public CombatSystem(Stats ownStats, HashSet<CombatCondition> conditions)
    {
        this.ownStats = ownStats;
        this.conditions = conditions;
        resCalc = new ResistanceCalculator(this);
    }
    public float CalculateDamage(Stats attackerStats, float damage)
    {
        float resistance = resCalc.SummedResistance(attackerStats, ownStats);
        return damage * (1 - resistance);
    }

    public bool ValidAttributeInContext(CombatAttribute attribute)
    {
        for (int i = 0; i < attribute.conditions.Length; i++)
        {
            if (!conditions.TryGetValue(attribute.conditions[i], out _))
            {
                return false;
            }
        }
        return true;
    }
}
