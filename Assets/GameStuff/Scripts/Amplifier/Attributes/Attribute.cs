using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/Attribute")]
public class Attribute : ScriptableObject
{
    [Tooltip("Name is used in descriptions, this field can override that")]
    public string displayName;
    public string description;
    public bool invertedCalculation;
    public override string ToString()
    {
        if (displayName.Length > 0)
        {
            return displayName;
        }
        return name;
    }
}
