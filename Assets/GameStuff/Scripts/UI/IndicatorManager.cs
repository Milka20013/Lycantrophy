using System.Collections;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager instance;
    [SerializeField] private GameObject indicator;

    private Transform currentParent;
    private IEnumerator levitate;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of indicatorManager");
        }
        instance = this;
        levitate = LevitateIndicator();

    }

    public void ShowIndicator(Transform indicatorOrigin)
    {
        if (currentParent == indicatorOrigin)
        {
            return;
        }
        currentParent = indicatorOrigin;
        indicator.transform.SetParent(indicatorOrigin);
        indicator.transform.position = indicatorOrigin.position;
        indicator.SetActive(true);
        StartCoroutine(levitate);
    }

    public void HideIndicator()
    {
        indicator.transform.SetParent(transform);
        currentParent = transform;
        indicator.SetActive(false);
        StopCoroutine(levitate);
    }

    public void HideIndicator(Transform originTransform)
    {
        if (currentParent == originTransform)
        {
            HideIndicator();
        }
    }
    public Transform GetCurrentParent()
    {
        return currentParent;
    }
    IEnumerator LevitateIndicator()
    {
        bool up = true;
        for (; ; )
        {
            if (up)
            {
                indicator.transform.Translate(0.2f * Time.deltaTime * Vector3.up);
                if (indicator.transform.position.y >= currentParent.position.y + 0.1f)
                {
                    up = false;
                }
            }
            else
            {
                indicator.transform.Translate(0.2f * Time.deltaTime * Vector3.down);
                if (indicator.transform.position.y <= currentParent.position.y - 0.1f)
                {
                    up = true;
                }
            }

            yield return null;
        }
    }
}
