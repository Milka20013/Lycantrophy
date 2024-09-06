using System.Collections.Generic;

public class CombatSystem
{
    private Stats ownStats;
    public HashSet<CombatCondition> conditions = new();

    private ResistanceCalculator resCalc;
    private DamageMultiplierCalculator dmgCalc;

    public CombatSystem(Stats ownStats, HashSet<CombatCondition> conditions)
    {
        this.ownStats = ownStats;
        this.conditions = conditions;
        resCalc = new(this);
        dmgCalc = new(this);
    }
    public float CalculateDamage(Stats attackerStats, float damage)
    {
        float resistance = resCalc.SummedResistance(attackerStats, ownStats);
        float damageMultiplier = dmgCalc.CalculateDamageMultiplier(attackerStats, ownStats);
        return damage * (1 - resistance) * damageMultiplier;
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
