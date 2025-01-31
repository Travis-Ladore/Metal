using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadBobSystem : MonoBehaviour
{
    [Range(0.001f, 0.01f)]
    public float Amount = 0.002f;
    [Range(10f, 100f)]
    public float Smooth = 10f;

    public float baseFrequency = 10f; // Base frequency for walking speed
    public float speedMultiplier = 0.1f; // How much speed affects frequency

    private Vector3 StartPos;
    public UnityEvent onFootStep;

    private Movement playerMovement; // Reference to the Movement script
    private float currentFrequency;
    private float Sin;
    private bool isTriggered;

    void Start()
    {
        StartPos = transform.localPosition;

        // Get reference to the player's Movement script
        playerMovement = GetComponentInParent<Movement>();
        if (playerMovement == null)
        {
            Debug.LogError("Movement script not found! Ensure the HeadBobSystem is a child of the player object.");
        }
    }

    void Update()
    {
        if (playerMovement != null)
        {
            // Adjust frequency based on player speed
            currentFrequency = baseFrequency + (playerMovement.moveSpeed * speedMultiplier);
        }

        CheckForHeadBobTrigger();
        StopHeadBob();
    }

    private void CheckForHeadBobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        // Only trigger head bobbing if the player is grounded, moving, and not sliding or crouching
        if (inputMagnitude > 0 && playerMovement.grounded && !playerMovement.sliding && !playerMovement.crouching)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * currentFrequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * currentFrequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        Sin = Mathf.Sin(Time.time * currentFrequency);

        float footstepThreshold = 0.95f; // Threshold for triggering footstep sound
        if (Sin > footstepThreshold && !isTriggered)
        {
            isTriggered = true;

            // Only invoke footstep sound if the player is grounded
            if (playerMovement.grounded)
            {
                Debug.Log("Footstep triggered");
                onFootStep.Invoke();
            }
        }
        else if (isTriggered && Sin < -footstepThreshold)
        {
            isTriggered = false;
        }

        return pos;
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }
}
