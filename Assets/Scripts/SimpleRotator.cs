using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour {

    [SerializeField]
    private Vector3 rotSpeed;



    private void Update()
    {
        transform.localEulerAngles += rotSpeed * Time.deltaTime;
    }

}
