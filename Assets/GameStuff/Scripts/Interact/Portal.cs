using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactable
{
    [SerializeField] private string sceneName;
    [SerializeField] private int levelRequirement;
    public override void Interact(Player player)
    {
        base.Interact(player);
        if (player.TryGetComponent(out Levelling levelling))
        {
            if (levelling.currentLevel < levelRequirement)
            {
                return;
            }
            if (!PopupManager.instance.Show("Do you want to teleport to Farmland?"))
            {
                //the popup didn't show
                return;
            }
            PopupManager.instance.OnAgree += OnAgree;
        }
    }

    public void OnAgree()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Teleport to : " + sceneName);
    }
}
