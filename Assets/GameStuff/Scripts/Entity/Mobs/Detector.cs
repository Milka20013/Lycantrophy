using UnityEngine;

public class Detector : MonoBehaviour
{
    public LayerMask targetLayer = 1 << 12;

    public float detectionRange = 20f;

    public float sleepInterval = 0.5f;
    private double previousTime;
    public bool autoDetect = false;


    public int maxTargets = 10;

    public delegate void DetectionHandler(Collider collider);
    public DetectionHandler onDetection;


    void Update()
    {
        if (autoDetect && Time.timeAsDouble - previousTime >= sleepInterval)
        {
            previousTime = Time.timeAsDouble;
            DetectTargets();
        }
    }

    public bool TryDetectTargets(Transform detectOrigin, out GameObject[] targets, float detectionRange = 0f)
    {
        targets = null;
        if (detectionRange == 0)
        {
            detectionRange = this.detectionRange;
        }
        Collider[] colliders = Physics.OverlapSphere(detectOrigin.position, detectionRange, targetLayer);
        if (colliders.Length > 0)
        {
            targets = new GameObject[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                targets[i] = colliders[i].gameObject;
            }
            return true;
        }
        return false;
    }
    public void DetectTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
        if (colliders == null || colliders.Length == 0)
        {
            return;
        }
        onDetection?.Invoke(colliders[0]);
    }
}
