using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowScript : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("InputSystem")]
    
    private InputAction fire;
    public PlayerInputActions playerControls;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardsForce;
    bool readyToThrow;
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
    }
    private void OnDisable()
    {
        fire.Disable();
    }

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if(fire.triggered && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection  * throwForce + transform.up * throwUpwardsForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        Explodable explodable = projectile.GetComponent<Explodable>();
        if (explodable != null)
        {
            explodable.MarkAsThrown();
        }

        totalThrows--;

        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
