using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SetBonusManager", menuName = "Manager/SetBonusManager")]
public class SetBonusManager : ScriptableObject
{
    private SetBonusBlueprint[] setBonuses;

    private void Awake()
    {
        FindBonuses();
    }

    public void FindBonuses()
    {
        setBonuses = Resources.LoadAll<SetBonusBlueprint>("SetBonuses");
    }

    public SetBonusBlueprint GetBlueprint(SetTag tag)
    {
        for (int i = 0; i < setBonuses.Length; i++)
        {
            if (setBonuses[i].tag == tag)
            {
                return setBonuses[i];
            }
        }
        Debug.LogError("Tag was not found. Try refreshing bonuses in the setBonuses, or create a setbonus with tag " + tag.setName);
        return null;
    }
}
