using UnityEngine;


[CreateAssetMenu(fileName = "ItemBlueprint", menuName = "ItemBlueprint/BasicItem")]
public class ItemBlueprint : ScriptableObject
{
    public string id;
    public string itemName;
    public int stackSize;
    public string basicDescription;
    public GameObject prefab;
    public Sprite[] sprites;
}
