
using UnityEditor;


[CustomEditor(typeof(ItemBlueprint), true)]

public class ItemBlueprintEditor : Editor
{
    public void GenerateId(ref ItemBlueprint item)
    {
        item.id = System.Guid.NewGuid().ToString();
    }
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        ItemBlueprint item = (ItemBlueprint)target;
        if (item.id == null || item.id == "")
        {
            GenerateId(ref item);
        }
    }
}

[CustomEditor(typeof(ProductBlueprint))]
public class ProductEditor : ItemBlueprintEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ProductBlueprint bp = (ProductBlueprint)target;
        if (bp.quantity > bp.stackSize)
        {
            EditorGUILayout.HelpBox("Quantity should be lower than stacksize", MessageType.Warning);
        }
    }
}

