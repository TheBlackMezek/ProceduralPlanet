using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OctasphereTester : MonoBehaviour {

    [SerializeField]
    private int subdivisions = 0;
    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private MeshFilter meshFilter;



    private void Awake()
    {
        meshFilter.mesh = OctasphereCreator.Create(subdivisions, radius);
    }

}
