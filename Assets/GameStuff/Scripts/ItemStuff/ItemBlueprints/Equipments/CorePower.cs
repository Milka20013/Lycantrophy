using UnityEngine;

[CreateAssetMenu(fileName = "CorePower", menuName = "CorePower/Power")]
public class CorePower : ScriptableObject
{
    public Amplifier amplifier;
    public float[] possibleValues;

    public Amplifier Reroll()
    {
        Amplifier amp = new Amplifier(amplifier);
        amp.value = Randomizer.GetRandomElementFromFairTable(possibleValues);
        return amp;
    }
}
