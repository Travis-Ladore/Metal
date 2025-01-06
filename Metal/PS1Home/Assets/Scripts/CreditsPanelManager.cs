using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel; // Reference to the Credits Panel
    [SerializeField] private GameObject controlsPanel; // Reference to the Controls Panel

    private void Update()
    {
        // Close the credits panel when Escape is pressed
        if (creditsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCredits();
        }

        // Close the controls panel when Escape is pressed
        if (controlsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseControls();
        }
    }

    // Open the Credits Panel
    public void OpenCredits()
    {
        creditsPanel.SetActive(true); // Show the credits panel
        Time.timeScale = 0f; // Pause the game (optional)
    }

    // Close the Credits Panel
    public void CloseCredits()
    {
        creditsPanel.SetActive(false); // Hide the credits panel
        Time.timeScale = 1f; // Resume the game (optional)
    }

    // Open the Controls Panel
    public void OpenControls()
    {
        controlsPanel.SetActive(true); // Show the controls panel
        Time.timeScale = 0f; // Pause the game (optional)
    }

    // Close the Controls Panel
    public void CloseControls()
    {
        controlsPanel.SetActive(false); // Hide the controls panel
        Time.timeScale = 1f; // Resume the game (optional)
    }
}
