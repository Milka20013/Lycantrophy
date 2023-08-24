
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EssenceBlueprint", menuName = "ItemBlueprint/Essence")]
public class EssenceBlueprint : ItemBlueprint
{
    public Amplifier[] amplifiers;
    [Tooltip("Place every CorePower here that can be assigned to the item")]
    public CorePower[] possibleCorePowers;
    public int maxNumberOfCorePowers;

    public List<CorePower> AddCorePowers(CorePower[] existingCorePowers, int numberOfPowersToAdd = 1)
    {
        List<CorePower> newCorePowers = new();
        for (int i = 0; i < numberOfPowersToAdd; i++)
        {
            newCorePowers.Add(Randomizer.GetRandomElementFromFairTableExcept(possibleCorePowers, existingCorePowers));
        }
        return newCorePowers;
    }

    public List<CorePower> RerollCorePowers(int numberOfPowers = 1)
    {
        List<CorePower> corePowers = new();
        for (int i = 0; i < numberOfPowers; i++)
        {
            corePowers.Add(Randomizer.GetRandomElementFromFairTable(possibleCorePowers));
        }
        return corePowers;
    }
}
