using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlatosphereNode : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private GameObject nodePrefab;

    private float sphereRadius;
    private int meshSubdivisions;
    private Vector3[] corners;
    private PlanetNoise noiseMaker;

    private PlatosphereNode[] children;





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

    public void Subdivide()
    {
        children = new PlatosphereNode[4];
        
        for(int i = 0; i < 4; ++i)
        {
            GameObject child = Instantiate(nodePrefab);
            child.transform.position = transform.position;
            child.transform.rotation = transform.rotation;
            child.transform.parent = transform;
            children[i] = child.GetComponent<PlatosphereNode>();
        }

        Vector3 mid01 = corners[1] - corners[0];
        mid01 = corners[0] + mid01.normalized * (mid01.magnitude / 2f);
        Vector3 mid02 = corners[2] - corners[0];
        mid02 = corners[0] + mid02.normalized * (mid02.magnitude / 2f);
        Vector3 mid12 = corners[2] - corners[1];
        mid12 = corners[1] + mid12.normalized * (mid12.magnitude / 2f);
        Debug.Log(corners[0] + " " + mid01 + " " + corners[1]);
        Vector3[] corners0 = new Vector3[] { corners[0], mid01, mid02 }; //top
        Vector3[] corners1 = new Vector3[] { mid01, corners[1], mid12 }; //left
        Vector3[] corners2 = new Vector3[] { mid02, mid12, corners[2] }; //right
        Vector3[] corners3 = new Vector3[] { mid02, mid01, mid12 }; //center

        children[0].Initialize(sphereRadius, meshSubdivisions, corners0, noiseMaker);
        children[1].Initialize(sphereRadius, meshSubdivisions, corners1, noiseMaker);
        children[2].Initialize(sphereRadius, meshSubdivisions, corners2, noiseMaker);
        children[3].Initialize(sphereRadius, meshSubdivisions, corners3, noiseMaker);

        meshRenderer.enabled = false;
    }

    private void Recombine()
    {
        for (int i = 0; i < 4; ++i)
            children[i].Destroy();

        children = null;

        meshRenderer.enabled = true;
    }

    public void Destroy()
    {
        if (children != null)
            Recombine();

        Destroy(gameObject);
    }

}
