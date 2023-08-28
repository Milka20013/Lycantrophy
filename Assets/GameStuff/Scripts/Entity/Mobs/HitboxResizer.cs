using UnityEngine;
using UnityEngine.AI;

public class HitboxResizer : MonoBehaviour
{
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
    }
}
