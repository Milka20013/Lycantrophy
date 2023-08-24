using UnityEngine;
using UnityEngine.EventSystems;


public class Product : MonoBehaviour, IPointerDownHandler
{
    private ItemUI itemUI;
    private ProductBlueprint productBP;
    private Shop shop;
    private Player player;
    private void Awake()
    {
        itemUI = GetComponent<ItemUI>();
    }
    private void Start()
    {
        shop = itemUI.inventory.GetComponent<Shop>();
        player = shop.player;
        productBP = itemUI.inventory.itemManager.GetItemBlueprint<ProductBlueprint>(itemUI.itemStack);
        itemUI.basicDescription = productBP.basicDescription + "\n" + "<color=yellow>" + productBP.price.ToString() + "</color>";
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (productBP.price > player.playerInventory.GetCurrency())
            {
                shop.ShowErrorText();
            }
            else
            {
                player.playerInventory.AddCurrency(-productBP.price);
                player.playerInventory.AddItem(productBP.product, productBP.quantity);
            }
        }
    }
}
