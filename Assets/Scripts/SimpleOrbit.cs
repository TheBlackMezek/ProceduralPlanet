using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbit : MonoBehaviour {

    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Vector3 rotationAxis;
    [SerializeField]
    private float rotationRate;



    private void Update()
    {
        transform.RotateAround(parent.transform.position, rotationAxis, rotationRate * Time.deltaTime);
    }

}
