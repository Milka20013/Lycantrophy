using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/Attribute")]
public class Attribute : ScriptableObject
{
    public override string ToString()
    {
        return name;
    }
}
