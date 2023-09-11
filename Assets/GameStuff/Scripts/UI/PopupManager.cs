using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI description;
    public delegate void Answear();
    public Answear OnAgree;
    public Answear OnDecline;
    [HideInInspector] public bool hideAfterAnswear;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            Debug.LogError("Multiple instances of PopupManager");
        }
        else
        {
            instance = this;
        }
        hideAfterAnswear = true;
        Hide();
    }

    public bool ActiveSelf()
    {
        return popup.activeSelf;
    }
    public bool Show(string text)
    {
        if (popup.activeSelf)
        {
            return false;
        }
        description.text = text;
        popup.SetActive(true);
        return true;
    }

    public void Hide()
    {
        popup.SetActive(false);
        ClearMethodCalls();
    }

    public void Agree()
    {
        OnAgree?.Invoke();
        if (hideAfterAnswear)
        {
            Hide();
        }

    }
    public void Cancel()
    {
        OnDecline?.Invoke();
        if (hideAfterAnswear)
        {
            Hide();
        }

    }

    public void ClearMethodCalls()
    {
        OnAgree = null;
        OnDecline = null;
    }

}
