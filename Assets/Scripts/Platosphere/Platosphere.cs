using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platosphere : MonoBehaviour {

    public enum PlatosphereType
    {
        TETRAHEDRON,
        OCTAHEDRON,
        ICOSAHEDRON
    }

    private Vector3[] tetrahedron =
    {
        new Vector3(1, 1, 1),
        new Vector3(-1, -1, 1),
        new Vector3(-1, 1, -1),
        new Vector3(1, -1, -1)
    };

    private Vector3[] octahedron =
    {
        Vector3.up,
        Vector3.forward,
        -Vector3.right,
        -Vector3.forward,
        Vector3.right,
        -Vector3.up
    };

    private Vector3[] icosahedron;

    [SerializeField]
    private PlatosphereType type = PlatosphereType.ICOSAHEDRON;
    [SerializeField]
    private float radius = 1f;
    [SerializeField, Range(0, 8), Tooltip("Range due to Unity mesh vertex count limits")]
    private int nodeSubdivisions;
    [SerializeField]
    private GameObject nodePrefab;

    [Header("Noise Settings")]

    [SerializeField]
    private int octaves;
    [SerializeField]
    private float frequency = 1f;
    [SerializeField]
    private float lacunarity = 2f;
    [SerializeField]
    private float amplitude = 1f;
    [SerializeField]
    private float persistence = 0.5f;
    [SerializeField]
    private int seed;

    private PlatosphereNode[] nodes;
    private PlanetNoise noiseMaker;



    private void OnValidate()
    {
        if (octaves < 1)
            octaves = 1;
    }

    private void Awake()
    {
        //icosahedronCorners cannot be initialized up above because it has to use Mathf.Sqrt()
        float t = (1f + Mathf.Sqrt(5f)) / 2f;
        icosahedron = new Vector3[]
        {
            new Vector3(-1, t, 0),
            new Vector3(1, t, 0),
            new Vector3(-1, -t, 0),
            new Vector3(1, -t, 0),

            new Vector3(0, -1, t),
            new Vector3(0, 1, t),
            new Vector3(0, -1, -t),
            new Vector3(0, 1, -t),

            new Vector3(t, 0, -1),
            new Vector3(t, 0, 1),
            new Vector3(-t, 0, -1),
            new Vector3(-t, 0, 1)
        };

        noiseMaker = new PlanetNoise(octaves, frequency, lacunarity, amplitude, persistence, seed);
    }

    private void Start()
    {
        Vector3 myPos = transform.position;

        switch (type)
        {
            case PlatosphereType.TETRAHEDRON:

                nodes = new PlatosphereNode[4];
                for(int i = 0; i < 4; ++i)
                {
                    GameObject node = Instantiate(nodePrefab);
                    node.transform.position = myPos;
                    node.transform.parent = transform;

                    nodes[i] = node.GetComponent<PlatosphereNode>();
                    switch(i)
                    {
                        case 0:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { tetrahedron[2], tetrahedron[0], tetrahedron[1] }, noiseMaker);
                            break;
                        case 1:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { tetrahedron[0], tetrahedron[2], tetrahedron[3] }, noiseMaker);
                            break;
                        case 2:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { tetrahedron[0], tetrahedron[3], tetrahedron[1] }, noiseMaker);
                            break;
                        case 3:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { tetrahedron[2], tetrahedron[1], tetrahedron[3] }, noiseMaker);
                            break;
                    }
                }
                break;
                

            case PlatosphereType.OCTAHEDRON:
                nodes = new PlatosphereNode[8];
                for (int i = 0; i < 8; ++i)
                {
                    GameObject node = Instantiate(nodePrefab);
                    node.transform.position = myPos;
                    node.transform.parent = transform;

                    nodes[i] = node.GetComponent<PlatosphereNode>();
                    switch (i)
                    {
                        case 0:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[0], octahedron[1], octahedron[2] }, noiseMaker);
                            break;
                        case 1:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[0], octahedron[2], octahedron[3] }, noiseMaker);
                            break;
                        case 2:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[0], octahedron[3], octahedron[4] }, noiseMaker);
                            break;
                        case 3:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[0], octahedron[4], octahedron[1] }, noiseMaker);
                            break;
                        case 4:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[5], octahedron[4], octahedron[3] }, noiseMaker);
                            break;
                        case 5:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[5], octahedron[3], octahedron[2] }, noiseMaker);
                            break;
                        case 6:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[5], octahedron[2], octahedron[1] }, noiseMaker);
                            break;
                        case 7:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { octahedron[5], octahedron[1], octahedron[4] }, noiseMaker);
                            break;
                    }
                }
                break;
            
            
            case PlatosphereType.ICOSAHEDRON:

                nodes = new PlatosphereNode[20];
                for (int i = 0; i < 20; ++i)
                {
                    GameObject node = Instantiate(nodePrefab);
                    node.transform.position = myPos;
                    node.transform.parent = transform;

                    nodes[i] = node.GetComponent<PlatosphereNode>();
                    switch (i)
                    {
                        case 0:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[0], icosahedron[5], icosahedron[11] }, noiseMaker);
                            break;
                        case 1:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[0], icosahedron[1], icosahedron[5] }, noiseMaker);
                            break;
                        case 2:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[0], icosahedron[7], icosahedron[1] }, noiseMaker);
                            break;
                        case 3:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[0], icosahedron[10], icosahedron[7] }, noiseMaker);
                            break;
                        case 4:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[0], icosahedron[11], icosahedron[10] }, noiseMaker);
                            break;


                        case 5:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[1], icosahedron[9], icosahedron[5] }, noiseMaker);
                            break;
                        case 6:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[5], icosahedron[4], icosahedron[11] }, noiseMaker);
                            break;
                        case 7:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[11], icosahedron[2], icosahedron[10] }, noiseMaker);
                            break;
                        case 8:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[10], icosahedron[6], icosahedron[7] }, noiseMaker);
                            break;
                        case 9:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[7], icosahedron[8], icosahedron[1] }, noiseMaker);
                            break;


                        case 10:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[3], icosahedron[4], icosahedron[9] }, noiseMaker);
                            break;
                        case 11:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[3], icosahedron[2], icosahedron[4] }, noiseMaker);
                            break;
                        case 12:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[3], icosahedron[6], icosahedron[2] }, noiseMaker);
                            break;
                        case 13:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[3], icosahedron[8], icosahedron[6] }, noiseMaker);
                            break;
                        case 14:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[3], icosahedron[9], icosahedron[8] }, noiseMaker);
                            break;


                        case 15:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[4], icosahedron[5], icosahedron[9] }, noiseMaker);
                            break;
                        case 16:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[2], icosahedron[11], icosahedron[4] }, noiseMaker);
                            break;
                        case 17:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[6], icosahedron[10], icosahedron[2] }, noiseMaker);
                            break;
                        case 18:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[8], icosahedron[7], icosahedron[6] }, noiseMaker);
                            break;
                        case 19:
                            nodes[i].Initialize(radius, nodeSubdivisions, new Vector3[] { icosahedron[9], icosahedron[1], icosahedron[8] }, noiseMaker);
                            break;
                    }
                }
                break;
        }
    }

}
