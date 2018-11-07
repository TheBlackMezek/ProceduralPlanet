using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFlycam : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float mouseSensitivity;
    [SerializeField]
    private float rotateSensitivity;
    [SerializeField]
    private float moveSpeedChangeMultiplier;
    [SerializeField]
    private Platosphere parent;
    [SerializeField]
    private Rigidbody rb;

    private float azimuthAngle;
    private float zenithAngle;
    private float radialDistance;



    private void Start()
    {
        //radialDistance = parent.Radius + parent.Radius * 0.1f;
        transform.parent = parent.transform;
        //SetPosOnSphere();
        //transform.LookAt(parent.transform);
        //transform.localEulerAngles += Vector3.right * -90f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        moveSpeed += moveSpeed * moveSpeedChangeMultiplier * Input.GetAxis("Mouse ScrollWheel");

        Vector3 moveVec = Vector3.zero;

        moveVec += transform.forward * Input.GetAxis("ForeBack") * moveSpeed;
        moveVec += transform.right * Input.GetAxis("Horizontal") * moveSpeed;
        moveVec += transform.up * Input.GetAxis("Vertical") * moveSpeed;

        transform.position += moveVec;



        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rb.angularVelocity = transform.right * -my * mouseSensitivity + transform.up * mx * mouseSensitivity;

        if (Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(transform.forward * (-rotateSensitivity));
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(transform.forward * rotateSensitivity);
        }



        //SetPosOnSphere();
    }

    private void SetPosOnSphere()
    {
        Vector3 spherePos;
        spherePos.x = radialDistance * Mathf.Cos(azimuthAngle) * Mathf.Sin(zenithAngle);
        spherePos.y = radialDistance * Mathf.Sin(azimuthAngle) * Mathf.Sin(zenithAngle);
        spherePos.z = radialDistance * Mathf.Cos(zenithAngle);

        transform.localPosition = spherePos;
    }

}
