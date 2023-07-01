using System;
using TMPro;
using UnityEngine;

//this class helps the button to choose the stat 
[Serializable]
public class StatSelector : MonoBehaviour
{
    public Attribute attributeType;
    public float value;
    public TextMeshProUGUI textToUpdate;
}
