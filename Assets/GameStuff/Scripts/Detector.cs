using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public float detectionRange;

    public float sleepInterval;
    private double previousTime;
    public bool autoDetect = false;

    public LayerMask targetLayer;

    [HideInInspector] public GameObject[] targets;
    public int maxTargets = 10;

    public delegate void DetectionHandler(bool hasTarget);
    public DetectionHandler onDetection;

    private void Awake()
    {
        targets = new GameObject[maxTargets];
    }

    void Update()
    {
        if (autoDetect && Time.timeAsDouble - previousTime >= sleepInterval)
        {
            previousTime = Time.timeAsDouble;
            TryDetectTargets();
        }
    }

    public bool TryDetectTargets(out GameObject[] targets, float detectionRange = 0f)
    {
        targets = null;
        if (detectionRange == 0)
        {
            detectionRange = this.detectionRange;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
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
    public void TryDetectTargets()
    {
        targets = new GameObject[maxTargets];
        int j = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                targets[i] = colliders[i].gameObject;
                j++;
                if (j >= maxTargets)
                {
                    break;
                }
            }
        }
        onDetection?.Invoke(colliders.Length > 0);
    }
}
