using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeEffect
{
    InstantHeal
}
public abstract class ConsumableItem : ScriptableObject
{
    public ConsumeEffect consumeEffect;
    public string description;
    public float consumeValue;

    public string FullDescription()
    {
        return description + ": " + consumeValue.ToString();
    }
    public abstract void ConsumeItem(Player player);
}
