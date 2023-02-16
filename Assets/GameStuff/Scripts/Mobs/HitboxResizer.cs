using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
public class HitboxResizer : MonoBehaviour
{
    private void Start()
    {
        BoxCollider coll = GetComponent<BoxCollider>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        coll.size = mesh.bounds.size;
        coll.center = mesh.bounds.center;
    }
}
