using TMPro;
using UnityEngine;

public class PlayerInventory : Inventory
{

    private float soulFragments;
    private Player player;

    //UI
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Awake()
    {
        player = GetComponent<Player>();
        UpdateText();
    }

    public void AddCurrency(float amount)
    {
        soulFragments += amount;
        UpdateText();
    }

    public float GetCurrency()
    {
        return soulFragments;
    }

    private void UpdateText()
    {
        currencyText.text = Mathf.Floor(soulFragments).ToString();
    }

    override public T GetOwner<T>() where T : class
    {
        if (typeof(T) == typeof(Player))
        {
            return player as T;
        }
        return null;
    }
}
