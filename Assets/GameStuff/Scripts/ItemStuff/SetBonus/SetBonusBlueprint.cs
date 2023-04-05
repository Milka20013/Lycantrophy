using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SetBonusBP",menuName ="SetBonus/Blueprint")]
public class SetBonusBlueprint : ScriptableObject
{
    public SetTag tag;
    public SetBonusVariable[] setBonusVariables;
}
