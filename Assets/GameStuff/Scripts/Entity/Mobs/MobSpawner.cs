using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawner : MonoBehaviour
{
    public Terrain terrain;
    //public GameObject mobPrefab;
    //public Transform player;
    [SerializeField] private bool debug;

    public float spawnMinDistanceFromPlayer;
    public float spawnMaxDistanceFromPlayer;
    public LayerMask layersToSpawnOn;

    public Vector3 mobOffset; //offset the position of the mob

    [Header("Pile spawning")]
    public GameObject pileContainer;
    private List<MobPile> mobPiles = new();

    private double previousTime = 0;
    private void Start()
    {
        for (int i = 0; i < pileContainer.transform.childCount; i++)
        {
            if (!pileContainer.transform.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }
            mobPiles.Add(pileContainer.transform.GetChild(i).GetComponent<MobPile>()); //get all of the mobPiles on the scene
        }
        SpawnMobsInPiles();
    }
    private void Update()
    {
        if (Time.timeAsDouble - previousTime >= 1)
        {
            previousTime = Time.timeAsDouble;
            for (int i = 0; i < mobPiles.Count; i++)
            {
                if (mobPiles[i].respawnDelay <= 0f)
                {
                    SpawnMobsInPile(mobPiles[i]);
                }
            }
        }
    }
    /*public void SpawnMob()
    {
        //not working properly
        //spwans mob in the range of player
        int parity1 = Random.value < 0.5f ? 1 : -1;
        float randomDistance1 = Random.Range(spawnMinDistanceFromPlayer, spawnMaxDistanceFromPlayer) * parity1; //Calculating random distances
        int parity2 = Random.value < 0.5f ? 1 : -1;                                                             //Which is between spawnmin and spawn max away
        float randomDistance2 = Random.Range(spawnMinDistanceFromPlayer, spawnMaxDistanceFromPlayer) * parity2; //From the projected player position
        Vector3 offset = new Vector3(randomDistance1, 0f, randomDistance2);


        Vector3 castingPoint = player.position + new Vector3(0f, 1000f, 0f) + offset;


        if (Physics.Raycast(castingPoint, Vector3.down, out RaycastHit hit, 1100f, layersToSpawnOn)) //casting the ray to spawn
        {
            GameObject mob = Instantiate(mobPrefab, hit.point + mobOffset, Quaternion.identity);
        }
        else
        {


        }
    }*/

    public void SpawnMobsInPiles()
    {
        for (int i = 0; i < mobPiles.Count; i++)
        {
            SpawnMobsInPile(mobPiles[i]);
        }
    }

    public void SpawnMobsInPile(MobPile mobPile)
    {
        //to-do : courutine
        int count;
        GameObject[] spawnedMobs;

        //which mob to spawn
        MobData mobData = Randomizer.GetRandomElementFromFairTable(mobPile.mobs);
        if (debug)
        {
            Debug.Log("Spawning: " + mobData.prefab);
        }
        GameObject prefab = mobData.prefab;

        //how much
        count = Random.Range(1, mobPile.maxSpawnBatchSize + 1);
        count = Mathf.Clamp(count, 0, mobPile.maxMobCount - mobPile.mobCount); //stay inside the boundaries of the pile
        spawnedMobs = new GameObject[count];

        for (int j = 0; j < count; j++)
        {
            spawnedMobs[j] = Instantiate(prefab, GetRandomPositionInPile(mobPile), Quaternion.identity);
            //Mob mobScr = spawnedMobs[j].GetComponent<Mob>();
            //mobScr.RegisterMobData(mobData);
            //var mesh = Instantiate(mobData.meshPrefab, spawnedMobs[j].transform.position, Quaternion.identity);
            //mesh.transform.SetParent(spawnedMobs[j].transform);
        }
        mobPile.RegisterMobs(spawnedMobs); //give information to the the pile that mobs were spawned
        mobPile.respawnDelay = mobPile.spawnTimer;
    }

    public Vector3 GetRandomPositionInPile(MobPile mobPile)
    {
        Vector2 circlePos = Random.insideUnitCircle * mobPile.circleRadius;
        float xPos = circlePos.x + mobPile.transform.position.x;
        float zPos = circlePos.y + mobPile.transform.position.z; //bit of changing the coordinate names
        float yPos = terrain.SampleHeight(new Vector3(xPos, 0, zPos));
        Vector3 position = new Vector3(xPos, yPos, zPos);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 3f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            Debug.Log("Couldn't find a position on the navmesh");
        }
        return position;
    }
}
