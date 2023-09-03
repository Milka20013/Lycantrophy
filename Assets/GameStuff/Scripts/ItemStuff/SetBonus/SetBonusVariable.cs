using System;
using UnityEngine;

[Serializable]
public class SetBonusVariable
{
    public Amplifier amplifier;
    [Tooltip("Defualt value is 3. How much tier is needed to gain this amplifier")]
    public int start = 3;
    [Tooltip("Defualt value is 3. The function is calculated like this: function(start) - function(offset - 1)")]
    public int offset = 3;
    [Tooltip("Defualt value is 1. Higher weights increase the bonus added after 3-6-9 jumps")]
    public float weight = 1;
}
