using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.AudioSettings;

public class MobSpawner : MonoBehaviour
{
    public Terrain terrain;
    public GameObject mobPrefab;
    public Transform player;

    public float spawnMinDistanceFromPlayer;
    public float spawnMaxDistanceFromPlayer;
    public LayerMask layersToSpawnOn;

    public Vector3 mobOffset; //offset the position of the mob

    [Header("Pile spawning")]
    public GameObject pileContainer;
    private MobPile[] mobPiles;

    private double previousTime = 0;
    private void Start()
    {
        mobPiles = new MobPile[pileContainer.transform.childCount];
        for (int i = 0; i < pileContainer.transform.childCount; i++)
        {
            mobPiles[i] = pileContainer.transform.GetChild(i).GetComponent<MobPile>(); //get all of the mobPiles on the scene
        }
        SpawnMobsInPiles();
    }
    private void Update()
    {
        if (Time.timeAsDouble - previousTime >= 1)
        {
            previousTime = Time.timeAsDouble;
            for (int i = 0; i < mobPiles.Length; i++)
            {
                if (mobPiles[i].respawnDelay <= 0f)
                {
                    SpawnMobsInPile(mobPiles[i]);
                }
            }
        }
    }
    public void SpawnMob()
    {
        //not working properly
        //spwans mob in the range of player
        int parity1 = Random.value < 0.5f? 1 : -1;
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
    }

    public void SpawnMobsInPiles()
    {
        for (int i = 0; i < mobPiles.Length; i++)
        {
            SpawnMobsInPile(mobPiles[i]);
        }
    }

    public void SpawnMobsInPile(MobPile mobPile)
    {
        //to-do : courutine
        float yOffset;
        int count;
        GameObject[] spawnedMobs;

        //which mob to spawn
        MobData mobData = GameManager.GetRandomElementFromFairTable(mobPile.mobs);
        GameObject prefab = mobData.prefab;

        //how much
        count = Random.Range(1, mobPile.maxSpawnBatchSize + 1);
        count = Mathf.Clamp(count, 0, mobPile.maxMobCount - mobPile.mobCount); //stay inside the boundaries of the pile
        spawnedMobs = new GameObject[count];
        for (int j = 0; j < count; j++)
        {
            yOffset = prefab.GetComponent<NavMeshAgent>().baseOffset;
            spawnedMobs[j] = Instantiate(prefab, GetRandomPositionInPile(mobPile,yOffset), Quaternion.identity);
            spawnedMobs[j].GetComponent<Mob>().RegisterMobData(mobData);
        }
        mobPile.RegisterMobs(spawnedMobs); //give information to the the pile that mobs were spawned
        mobPile.respawnDelay = mobPile.spawnTimer;
    }

    public Vector3 GetRandomPositionInPile(MobPile mobPile, float yOffset)
    {
        Vector2 circlePos = Random.insideUnitCircle * mobPile.circleRadius;
        float xPos = circlePos.x + mobPile.transform.position.x;
        float zPos = circlePos.y + mobPile.transform.position.z; //bit of changing the coordinate names
        float yPos = terrain.SampleHeight(new Vector3(xPos,0,zPos)); 
        Vector3 position = new Vector3(xPos,yPos + yOffset,zPos);
        return position;
    }
}
