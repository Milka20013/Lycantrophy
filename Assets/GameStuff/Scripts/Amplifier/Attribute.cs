using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/Attribute")]
public class Attribute : ScriptableObject
{
    public string description;
    public override string ToString()
    {
        return name;
    }
}
