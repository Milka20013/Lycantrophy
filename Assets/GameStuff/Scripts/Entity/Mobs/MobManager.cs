using UnityEngine;

public class MobManager : MonoBehaviour
{
    public static MobManager instance;
    public GameObject deathEffect;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of MobManager");
        }
        instance = this;
    }

    public void PlayDeathEffect(Vector3 position)
    {
        var effect = Instantiate(deathEffect, position, Quaternion.identity);
        Destroy(effect, 1.2f + Random.Range(0, 0.15f));
    }
}
