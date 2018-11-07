using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlatosphereNode : MonoBehaviour {

    [SerializeField, Tooltip("Max distance the player can be for the node to subdivide in percentage of the length of one side of the node")]
    private float percentDistToSubdivideAt;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Platosphere parentSphere;

    private float sphereRadius;
    private int meshSubdivisions;
    private Vector3[] corners;
    private PlanetNoise noiseMaker;

    private int level;

    private PlatosphereNode[] children;





    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(float radius, int subs, Vector3[] corners, PlanetNoise noiseMaker, Platosphere parentSphere, int level = 0)
    {
        sphereRadius = radius;
        meshSubdivisions = subs;
        this.corners = corners;
        this.noiseMaker = noiseMaker;
        this.parentSphere = parentSphere;
        this.level = level;

        Build();
    }

    public void NodeUpdate()
    {
        if(level < parentSphere.MaxNodeLevels)
        {
            bool inSubdivideRange = false;
            Vector3 myPos = transform.position;

            Vector3 corner0Pos = corners[0] * sphereRadius;
            Vector3 corner1Pos = corners[1] * sphereRadius;

            float distToSubdivide = Vector3.Distance(corner0Pos, corner1Pos) * (percentDistToSubdivideAt / 100f);

            Vector3 centerPoint = transform.TransformPoint(((corners[0] + corners[1] + corners[2]) / 3f) * sphereRadius);
            float dist = Vector3.Distance(parentSphere.Player.position, centerPoint);

            if (dist < distToSubdivide)
            {
                inSubdivideRange = true;

                if (children == null)
                    Subdivide();
            }

            if (!inSubdivideRange && children != null)
                Recombine();

            if (children != null)
            {
                for (int i = 0; i < 4; ++i)
                    children[i].NodeUpdate();
            }
        }
    }

    private void Build()
    {
        Mesh mesh = PlatosphereMeshMaker.BuildMesh(corners, meshSubdivisions, sphereRadius, noiseMaker, level);
        meshFilter.mesh = mesh;
    }

    public void Subdivide()
    {
        children = new PlatosphereNode[4];
        
        for(int i = 0; i < 4; ++i)
        {
            GameObject child = Instantiate(parentSphere.NodePrefab);
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
        
        Vector3[] corners0 = new Vector3[] { corners[0], mid01, mid02 }; //top
        Vector3[] corners1 = new Vector3[] { mid01, corners[1], mid12 }; //left
        Vector3[] corners2 = new Vector3[] { mid02, mid12, corners[2] }; //right
        Vector3[] corners3 = new Vector3[] { mid02, mid01, mid12 }; //center

        children[0].Initialize(sphereRadius, meshSubdivisions, corners0, noiseMaker, parentSphere, level + 1);
        children[1].Initialize(sphereRadius, meshSubdivisions, corners1, noiseMaker, parentSphere, level + 1);
        children[2].Initialize(sphereRadius, meshSubdivisions, corners2, noiseMaker, parentSphere, level + 1);
        children[3].Initialize(sphereRadius, meshSubdivisions, corners3, noiseMaker, parentSphere, level + 1);

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
