using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropList : MonoBehaviour
{
    [SerializeField] private GameObject iconObjectPrefab;
    [SerializeField] private GameObject imagePrefab;
    private GridLayoutGroup gridLayoutGroup;
    private List<GameObject> iconObjects = new();

    private void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }
    public void UpdatePanel(DroppableItem[] droppableItems)
    {
        PopulateDroppableItemsList(droppableItems);
        for (int i = 0; i < iconObjects.Count; i++)
        {
            iconObjects[i].SetActive(false);
        }
        for (int i = 0; i < droppableItems.Length; i++)
        {
            iconObjects[i].SetActive(true);
            PopulateIconObjectImages(iconObjects[i], droppableItems[i]);
            UpdateImages(iconObjects[i], droppableItems[i]);
        }
    }
    private void PopulateDroppableItemsList(DroppableItem[] droppableItems)
    {
        int numberOfIconObjects = iconObjects.Count;
        for (int i = 0; i < droppableItems.Length - numberOfIconObjects; i++)
        {
            var obj = Instantiate(iconObjectPrefab, transform);
            iconObjects.Add(obj);
        }

    }

    private void PopulateIconObjectImages(GameObject iconObject, DroppableItem droppableItem)
    {
        int childCount = iconObject.transform.childCount;
        for (int i = 0; i < droppableItem.item.sprites.Length - childCount; i++)
        {
            GameObject image = Instantiate(imagePrefab);
            image.transform.SetParent(iconObject.transform);
            image.transform.localPosition = Vector3.zero;
            image.transform.localScale = Vector3.one;
            image.GetComponent<RectTransform>().sizeDelta = gridLayoutGroup.cellSize;
        }
    }

    private void UpdateImages(GameObject iconObject, DroppableItem droppableItem)
    {
        for (int i = 0; i < iconObject.transform.childCount; i++)
        {
            iconObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < droppableItem.item.sprites.Length; i++)
        {
            iconObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < droppableItem.item.sprites.Length; i++)
        {
            iconObject.transform.GetChild(i).GetComponent<Image>().sprite = droppableItem.item.sprites[i];
        }
    }
}
