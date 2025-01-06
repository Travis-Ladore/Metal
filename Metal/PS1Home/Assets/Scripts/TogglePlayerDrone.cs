using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlayerDrone : MonoBehaviour
{
    public GameObject player;  // Reference to the player object
    public GameObject drone;   // Reference to the drone object

    private bool isPlayerActive = true;  // Tracks the state of the player (initially active)

    void Update()
    {
        // Check if the P key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleObjects();
        }
    }

    void ToggleObjects()
    {
        // Toggle the active state of player and drone
        if (isPlayerActive)
        {
            player.SetActive(false);
            drone.SetActive(true);
        }
        else
        {
            player.SetActive(true);
            drone.SetActive(false);
        }

        // Toggle the state
        isPlayerActive = !isPlayerActive;
    }
}
