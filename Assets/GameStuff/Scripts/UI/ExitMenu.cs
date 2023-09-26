using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitMenu : MonoBehaviour, IMenu
{
    [SerializeField] private GameObject panel;
    [SerializeField] Wiki wiki;
    private List<IMenu> menus = new();
    [SerializeField] private PlayerInput input;

    private void Awake()
    {
        menus = FindObjectsOfType<MonoBehaviour>().OfType<IMenu>().ToList();
        menus.Remove(this);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public bool IsOpen()
    {
        return panel.activeSelf;
    }

    public void OnTriggerMenu()
    {
        foreach (var item in menus)
        {
            if (item.IsOpen())
            {
                item.CloseMenu();
                return;
            }
        }
        if (IsOpen())
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        input.actions.FindActionMap("Player").Disable();
        input.actions.FindActionMap("Exit").Enable();
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseMenu()
    {
        input.actions.FindActionMap("Exit").Disable();
        input.actions.FindActionMap("Player").Enable();
        if (wiki != null)
        {
            wiki.ShowWiki(false);
        }
        panel.SetActive(false);
    }
}
