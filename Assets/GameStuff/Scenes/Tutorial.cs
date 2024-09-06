using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Levelling levelling;
    public Transform shopIndicatorTransform;
    public PlayerInventory playerInventory;
    public StatMenu statMenu;
    public GameObject shopMenu;
    public PopupManager popupManager;
    public GameObject[] popups;
    private int currentIndex = 0;
    private void Awake()
    {
        levelling.OnLevelUp += OnLevelUp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            ShowPopup(1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowPopup(2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowPopup(4);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowPopup(7);
        }
        if (IndicatorManager.instance.GetCurrentParent() == shopIndicatorTransform)
        {
            ShowPopup(12);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPopup(13);
        }
        if (popupManager.ActiveSelf())
        {
            ShowPopup(15);
        }

    }

    public void ClosePanels()
    {
        playerInventory.CloseMenu();
        statMenu.CloseMenu();
    }

    public void ShowPopup(int index)
    {
        if (index != currentIndex + 1)
        {
            return;
        }
        for (int i = 0; i < popups.Length; i++)
        {
            if (i != index)
            {
                popups[i].SetActive(false);
            }
        }
        currentIndex = index;
        popups[index].SetActive(true);
    }
    public void OnLevelUp(int _)
    {
        ShowPopup(3);
    }
}
