using UnityEngine;

[CreateAssetMenu(fileName = "ItemBlueprint", menuName = "ItemBlueprint/Product")]
public class ProductBlueprint : ItemBlueprint
{
    public float price;
    public ItemBlueprint product;
    public int quantity;
}
