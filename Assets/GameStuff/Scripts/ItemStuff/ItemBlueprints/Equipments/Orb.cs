using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemUI))]
[RequireComponent(typeof(DragAndDropItem))]
public class Orb : MonoBehaviour
{
    public ItemUI itemUI;
    public DragAndDropItem dragAndDrop;
    public OrbBlueprint orbBlueprint { get; private set; }
    void Start()
    {
        orbBlueprint = itemUI.inventory.itemManager.GetEquipmentItemBlueprint(itemUI.itemStack);
        itemUI.SetItemType(ItemType.Orb);
        RegisterEffects();
    }

    public void RegisterEffects()
    {
        if (orbBlueprint.hideDescription)
        {
            string[] desc = new string[orbBlueprint.amplifiers.Length];
            for (int i = 0; i < desc.Length; i++)
            {
                desc[i] = "??? ? ?";
            }
            itemUI.RegisterEffects(desc);
        }
        else
        {
            itemUI.RegisterEffects(orbBlueprint.amplifiers);
        }
    }
}
