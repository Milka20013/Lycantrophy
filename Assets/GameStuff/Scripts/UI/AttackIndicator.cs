using System.Collections;
using TMPro;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageRecievedText;
    [SerializeField] private TextMeshProUGUI damageDoneText;
    private Attacker attacker;
    [SerializeField] private TakeDamage takeDamage;
    private void Awake()
    {
        attacker = GetComponent<Attacker>();
        takeDamage.onHit += OnHit;
        attacker.onDamage += OnDamage;
    }

    public void OnDamage(float damage)
    {
        ShowText(damageDoneText, damage);
    }

    public void OnHit(float damage, GameObject _)
    {
        ShowText(damageRecievedText, damage);
    }

    public void ShowText(TextMeshProUGUI text, float damage)
    {
        if (text.gameObject.activeSelf)
        {
            return;
        }
        text.text = System.Math.Round(damage, 2).ToString();
        text.gameObject.SetActive(true);
        IEnumerator routine = HideText(text);
        StartCoroutine(routine);
    }

    public IEnumerator HideText(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(0.3f);
        text.CrossFadeAlpha(0, 0.45f, false);
        yield return new WaitForSeconds(0.45f);
        text.alpha = 255;
        text.gameObject.SetActive(false);
    }

}
