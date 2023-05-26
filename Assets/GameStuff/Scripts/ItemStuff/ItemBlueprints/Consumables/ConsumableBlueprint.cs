using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeEffect
{
    InstantHeal, Type1, Type2, Type3, Type4, Type5, Type6
}
public abstract class ConsumableBlueprint : ItemBlueprint
{
    public ConsumeEffect consumeEffect;
    public string effectDescription;
    public float consumeValue;

    public string FullDescription()
    {
        return effectDescription + ": " + consumeValue.ToString();
    }
    public abstract void ConsumeItem(HealthSystem healthSystem);
}
