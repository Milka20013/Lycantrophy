using UnityEngine;

public class Wiki : MonoBehaviour
{
    public GameObject wikiPanel;
    public void ShowWiki(bool active)
    {
        wikiPanel.SetActive(active);
    }
}
