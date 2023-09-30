using TMPro;
using UnityEngine;

public class MobInformationPanel : MonoBehaviour, ICloseableMenu
{
    public static MobInformationPanel instance;

    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private DropList dropList;

    private GameObject currentCaller;

    private HealthSystem currentHealthSystem;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of " + name);
        }
        instance = this;
    }

    public void CloseMenu()
    {
        if (!container.activeSelf)
        {
            return;
        }
        if (currentHealthSystem != null)
        {
            currentHealthSystem.onHealthChange -= OnHealthChange;
        }
        container.SetActive(false);
    }

    public void OpenMenu(GameObject caller, string name, HealthSystem healthSystem, DropTable dropTable)
    {
        if (container.activeSelf && currentCaller == caller)
        {
            return;
        }
        currentCaller = caller;
        container.SetActive(true);
        nameText.text = name;
        currentHealthSystem = healthSystem;
        currentHealthSystem.onHealthChange += OnHealthChange;
        OnHealthChange(currentHealthSystem.GetCurrentHealth());
        dropList.UpdatePanel(dropTable.GetDroppableItems().ToArray());

    }

    private void OnHealthChange(float currentHealth)
    {
        healthText.text = System.Math.Round(currentHealth, 2).ToString();
        if (currentHealth <= 0)
        {
            CloseMenu();
        }
    }

    public bool IsOpen()
    {
        return container.activeSelf;
    }
}
