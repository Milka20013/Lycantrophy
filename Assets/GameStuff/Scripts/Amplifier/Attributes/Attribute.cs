using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/Attribute")]
public class Attribute : ScriptableObject
{
    public string description;
    public bool invertedCalculation;
    public override string ToString()
    {
        return name;
    }
}
