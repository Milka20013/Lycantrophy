using Lycanthropy.Inventory;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingTool : MonoBehaviour
{
    public Player player;
    public PlayerInput input;
    public GameObject canvas;
    public TMP_InputField inputField;
    public ItemManager itemManager;

    private Levelling levelling;
    private PlayerInventory inventory;
    private void Start()
    {
        levelling = player.GetComponent<Levelling>();
        inventory = player.playerInventory;
    }

    public void OnShowCommandLine()
    {
        input.actions.FindActionMap("Player").Disable();
        input.actions.FindActionMap("Command").Enable();
        Cursor.lockState = CursorLockMode.None;
        canvas.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();

    }
    public void OnExit()
    {
        input.actions.FindActionMap("Player").Enable();
        input.actions.FindActionMap("Command").Disable();

        canvas.SetActive(false);
    }
    public void DoCommand(string text)
    {
        if (text == null)
        {
            return;
        }
        if (text.Length == 0)
        {
            return;
        }
        Regex regex = new(@"\w+|""[\w\s]*""");
        var regexTmp = regex.Matches(text);
        string[] tmp = new string[regexTmp.Count];
        for (int i = 0; i < regexTmp.Count; i++)
        {
            tmp[i] = regexTmp[i].ToString().Replace("\"", string.Empty);
        }
        string function = tmp[0];
        string[] args = new string[tmp.Length - 1];
        for (int i = 1; i < tmp.Length; i++)
        {
            args[i - 1] = tmp[i];
        }
        typeof(TestingTool).GetMethod(function)?.Invoke(this, new object[] { args });
    }
    public void AddExp(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }

        levelling.AddExp(float.Parse(args[0]));
    }
    public void AddItem(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }
        Item item = itemManager.GetItemByName(args[0]);
        if (item == null)
        {
            Debug.Log($"Item {args[0]} was not found");
            return;
        }
        inventory.AddItem(item, int.Parse(args[1]));
        Debug.Log("Item added");
    }

    public void AddMoney(string[] args)
    {
        //inventory.AddCurrency(money);
    }
}
