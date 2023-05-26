using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbBlueprint", menuName = "ItemBlueprint/Orb")]
public class OrbBlueprint : ItemBlueprint
{
    public Amplifier[] amplifiers;
    public SetTag tag;
    public int tier;
    public bool hideDescription;
}
