using TMPro;
using UnityEngine;


public class PlayerInventory : Inventory
{
    private float soulFragments;
    public OrbInventory orbInventory;
    private Player player;

    //UI
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Awake()
    {
        player = GetComponent<Player>();
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(stacksInInventory.Count);
        }
    }
    public void EquipItem(ItemStack itemStack)
    {
        orbInventory.EquipItem(itemStack);
        stacksInInventory.Remove(itemStack);
        itemSpawner.itemStacks = stacksInInventory;
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
