using UnityEditor;

[CustomEditor(typeof(Inventory), true)]
public class InventoryEditor : Editor
{
    public void GenerateGUID(ref Inventory inv)
    {
        inv.id = System.Guid.NewGuid().ToString();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Inventory inventroy = (Inventory)target;
        if (inventroy.id == null || inventroy.id == "")
        {
            GenerateGUID(ref inventroy);
        }
    }
}