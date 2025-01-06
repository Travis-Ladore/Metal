using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel; // The pause menu panel
    [SerializeField] private MonoBehaviour playerMovementScript; // Reference to the player movement script
    [SerializeField] private MonoBehaviour cameraLookScript; // Reference to the camera look script

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        // Disable player movement and camera look
        playerMovementScript.enabled = false;
        cameraLookScript.enabled = false;

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show the pause menu panel
        pauseMenuPanel.SetActive(true);

        // Pause time (optional)
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        // Enable player movement and camera look
        playerMovementScript.enabled = true;
        cameraLookScript.enabled = true;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hide the pause menu panel
        pauseMenuPanel.SetActive(false);

        // Resume time (optional)
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); // Quits the application
    }
}
