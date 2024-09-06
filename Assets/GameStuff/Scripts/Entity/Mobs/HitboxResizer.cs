using UnityEngine;
using UnityEngine.AI;

public class HitboxResizer : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    private void Start()
    {
        BoxCollider coll = GetComponent<BoxCollider>();
        SkinnedMeshRenderer mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        bool hasAgent = TryGetComponent(out NavMeshAgent agent);
        if (hasAgent)
        {
            float offsetY = agent.height / 2;
            coll.center = new Vector3(coll.center.x, coll.center.y + offsetY, coll.center.z);
        }
        coll.size = mesh.bounds.size;
        SpawnIndicatorOrigin(coll);
    }

    private void SpawnIndicatorOrigin(BoxCollider coll)
    {
        var mob = GetComponent<Mob>();
        if (mob.indicatorOriginTransform == null)
        {
            var origin = Instantiate(indicatorPrefab, mob.transform);
            origin.transform.SetLocalPositionAndRotation(new Vector3(0, coll.bounds.size.y / 2 + 0.15f, 0), Quaternion.identity);
            mob.indicatorOriginTransform = origin.transform;
        }
    }
}
