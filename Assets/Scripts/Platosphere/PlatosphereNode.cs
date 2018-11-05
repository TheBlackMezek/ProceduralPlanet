using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlatosphereNode : MonoBehaviour {

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float sphereRadius;
    private int meshSubdivisions;
    private Vector3[] corners;





    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float radius, int subs, Vector3[] corners)
    {
        sphereRadius = radius;
        meshSubdivisions = subs;
        this.corners = corners;

        Build();
    }

    private void Build()
    {
        Mesh mesh = PlatosphereMeshMaker.BuildMesh(corners, meshSubdivisions, sphereRadius);
        meshFilter.mesh = mesh;
    }

}
