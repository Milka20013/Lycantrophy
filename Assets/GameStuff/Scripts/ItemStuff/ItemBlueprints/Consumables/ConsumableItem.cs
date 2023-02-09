using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeEffect
{
    InstantHeal
}
public abstract class ConsumableItem : ItemBlueprint
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
