using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
    public Rigidbody ourDrone;
    public float upForce;
    public float movementForwardSpeed = 500;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;
    private float wantedYRotation;
    public float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;
    private Vector3 velocityToSmoothDampToZero;
    private float strafeMovement = 300;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    private AudioSource droneSound;
    public float sprintMultiplier = 1.5f;
    public bool isQPressed = false;
    public float freeFallSpeed = 2.0f;
    public float gravityValue = 98.1f;
    public float lightDownForce = -200;
    public float upCompensationForce = 500;
    public float soarForce = 450;
    public float sprintUpwardForce = 100.0f;
    public float sprintAscendUpwardForce = 100;
    public float additionalUpwardForceWhenMovingForward = 200f;
    public GameObject hook;
    public GameObject rope;
    private Quaternion hookOriginalRotation;
    void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        //droneSound = gameObject.transform.Find("drone_Sound").GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isQPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            isQPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateHook();
        }
        //DroneSound();
    }

    void FixedUpdate()
    {


        if (!isQPressed)
        {
            MovementUpDown();
            MovementForward();
            Rotation();
            ClampingSpeedValues();
            Strafe();

            ourDrone.AddRelativeForce(Vector3.up * upForce);
            ourDrone.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));

        }
        else
        {
            //ApplyFreeFall();
        }


    }

    void MovementUpDown()
    {
        bool isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetAxis("Vertical") > 0;
        bool isAscending = Input.GetKey(KeyCode.Space);
        bool isMovingForward = Input.GetAxis("Vertical") > 0;
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
            {
                ourDrone.velocity = ourDrone.velocity;
            }
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce = 281f;
            }
            if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Space) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce = 110;
            }
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                upForce = 410;
            }
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            upForce = 135;

        }


        if (isSprinting && isAscending)
        {
            upForce += sprintAscendUpwardForce; // Add additional upward force when sprinting and ascending
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            upForce = soarForce;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce = upCompensationForce;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            upForce = lightDownForce;
        }
        else if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.E) && Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            upForce = gravityValue;
        }

    }
    void MovementForward()
    {
        float finalMovementSpeed = movementForwardSpeed;
        bool isSprintingForward = false;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            finalMovementSpeed *= sprintMultiplier; // Apply sprint multiplier
                                                    // Check if moving forward while sprinting
            if (Input.GetAxis("Vertical") > 0)
            {
                isSprintingForward = true;
            }
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * finalMovementSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        }
        // Apply additional upward force if sprinting forward
        if (isSprintingForward)
        {
            upForce += sprintUpwardForce;
        }
    }

    void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5f, Time.deltaTime * 5f));

        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }

    void Strafe()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            ourDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * strafeMovement);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20 * Input.GetAxis("Horizontal"), ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }
    }

    void DroneSound()
    {
        //droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);

    }
    void ApplyGravity()
    {
        ourDrone.AddRelativeForce(Vector3.up * upForce);
        ourDrone.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));
    }
    void ApplyFreeFall()
    {
        ourDrone.AddRelativeForce(Vector3.down * upForce * freeFallSpeed);
        ourDrone.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways));
    }
    void ActivateHook()
    {
        // Check if the hook is already active
        if (!hook.activeSelf)
        {
            hookOriginalRotation = hook.transform.rotation;
            hook.SetActive(true);
            rope.SetActive(true);
        }
        else
        {
            hook.transform.rotation = hookOriginalRotation;
            hook.SetActive(false);
            rope.SetActive(false);
        }
    }
}