using UnityEngine;

public class MobPile : MonoBehaviour
{

    public MobData[] mobs;
    public int maxMobCount = 5;
    public float circleRadius = 3;
    public int maxSpawnBatchSize = 5;

    [HideInInspector] public int mobCount;

    public float spawnTimer = 10f;
    public float respawnDelay = 0f;


    private void Update()
    {
        if (respawnDelay > 0f)
        {
            respawnDelay -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
    public void RegisterMobs(GameObject[] mobs)
    {
        mobCount += mobs.Length;
        Mob[] mobScripts = new Mob[mobs.Length];
        for (int i = 0; i < mobs.Length; i++)
        {
            mobScripts[i] = mobs[i].GetComponent<Mob>();
            mobScripts[i].OnDeath += OnMobDeath;
        }
    }
    public void OnMobDeath(GameObject killer)
    {
        mobCount--;
    }
}
