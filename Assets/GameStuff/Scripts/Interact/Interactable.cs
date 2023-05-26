
using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject indicatorPosition;
    private bool isIndicatorShown;
    IEnumerator levitate;

    public virtual void Awake()
    {
        levitate = LevitateIndicator();
    }
    public virtual void Interact()
    {
        Debug.Log("hello from " + name);
    }

    public void ShowIndicator()
    {
        if (isIndicatorShown)
        {
            return;
        }
        isIndicatorShown = true;
        indicator.transform.position = indicatorPosition.transform.position;
        indicator.SetActive(true);
        StartCoroutine(levitate);
    }

    public virtual void HideIndicator()
    {
        if (!isIndicatorShown)
        {
            return;
        }
        isIndicatorShown = false;
        indicator.SetActive(false);
        StopCoroutine(levitate);
    }

    IEnumerator LevitateIndicator()
    {
        bool up = true;
        for (; ; )
        {
            if (up)
            {
                indicator.transform.Translate(Vector3.up * Time.deltaTime * 0.2f);
                if (indicator.transform.position.y >= indicatorPosition.transform.position.y + 0.1f)
                {
                    up = false;
                }
            }
            else
            {
                indicator.transform.Translate(Vector3.down * Time.deltaTime * 0.2f);
                if (indicator.transform.position.y <= indicatorPosition.transform.position.y - 0.1f)
                {
                    up = true;
                }
            }

            yield return null;
        }
    }
}
