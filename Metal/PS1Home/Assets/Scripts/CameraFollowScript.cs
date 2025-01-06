using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private Transform ourDrone;
    private Vector3 velocityCameraFollow;
    public float angle;
    public Vector3 behindPosition = new Vector3(0, 2, -4);
    private void Awake()
    {
        ourDrone = GameObject.FindGameObjectWithTag("Drone").transform;
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, ourDrone.transform.TransformPoint(behindPosition) + Vector3.up * Input.GetAxis("Vertical"), ref velocityCameraFollow, 0.1f);
        transform.rotation = Quaternion.Euler(new Vector3(angle, ourDrone.GetComponent<DroneMovementScript>().currentYRotation, 0));
    }

    private void LateUpdate()
    {
        ourDrone = GameObject.FindGameObjectWithTag("Drone").transform;
    }
}
