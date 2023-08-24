using Lycanthropy.Inventory;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Shop : Storage
{
    [SerializeField] private ItemDescriptionPanel ItemDescriptionPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private ProductBlueprint[] products;
    private void Start()
    {
        for (int i = 0; i < products.Length; i++)
        {
            inventory.AddItemNonDistributed(products[i], products[i].quantity);
        }
    }

    public void ShowErrorText()
    {
        if (errorText.gameObject.activeSelf)
        {
            return;
        }
        errorText.gameObject.SetActive(true);
        StartCoroutine(nameof(HideErrorText));
    }
    IEnumerator HideErrorText()
    {
        yield return new WaitForSeconds(0.5f);
        errorText.CrossFadeAlpha(0, 0.6f, false);
        yield return new WaitForSeconds(0.6f);
        errorText.alpha = 255;
        errorText.gameObject.SetActive(false);
    }
}
