using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OctaNode))]
public class OctaNodeTester : MonoBehaviour {

    [SerializeField]
    private OctaNode node;
    [SerializeField]
    private int divisions;
    [SerializeField]
    private Vector3[] corners;



    private void OnValidate()
    {
        if (corners.Length != 3)
            corners = new Vector3[3];
    }

    private void Start()
    {
        node.Build(corners, divisions);
    }

}
