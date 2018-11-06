using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlatosphereNode : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    private float sphereRadius;
    private int meshSubdivisions;
    private Vector3[] corners;
    private PlanetNoise noiseMaker;





    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float radius, int subs, Vector3[] corners, PlanetNoise noiseMaker)
    {
        sphereRadius = radius;
        meshSubdivisions = subs;
        this.corners = corners;
        this.noiseMaker = noiseMaker;

        Build();
    }

    private void Build()
    {
        Mesh mesh = PlatosphereMeshMaker.BuildMesh(corners, meshSubdivisions, sphereRadius, noiseMaker);
        meshFilter.mesh = mesh;
    }

}
