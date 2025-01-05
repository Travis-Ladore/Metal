using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float activationMass = 10f; // Minimum mass required to activate the plate
    public Transform door; // Reference to the door object
    public float doorMoveSpeed = 2f; // Speed at which the door moves
    public float doorOpenHeight = 5f; // Height the door will move to when activated

    private float totalMass = 0f;
    private Vector3 doorClosedPosition;
    private Vector3 doorOpenPosition;
    private bool isActivated = false;

    void Start()
    {
        if (door != null)
        {
            doorClosedPosition = door.position;
            doorOpenPosition = doorClosedPosition + Vector3.up * doorOpenHeight;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            totalMass += rb.mass;
            CheckActivation();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            totalMass -= rb.mass;
            CheckActivation();
        }
    }

    void CheckActivation()
    {
        if (totalMass >= activationMass && !isActivated)
        {
            isActivated = true;
            StartCoroutine(OpenDoor());
        }
        else if (totalMass < activationMass && isActivated)
        {
            isActivated = false;
            StartCoroutine(CloseDoor());
        }
    }

    System.Collections.IEnumerator OpenDoor()
    {
        while (door != null && door.position.y < doorOpenPosition.y)
        {
            door.position = Vector3.MoveTowards(door.position, doorOpenPosition, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    System.Collections.IEnumerator CloseDoor()
    {
        while (door != null && door.position.y > doorClosedPosition.y)
        {
            door.position = Vector3.MoveTowards(door.position, doorClosedPosition, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
