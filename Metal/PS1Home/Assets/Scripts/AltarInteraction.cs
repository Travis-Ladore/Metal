using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class AltarInteraction : MonoBehaviour
{
    public GameObject inputCanvas; // Reference to the canvas
    public TMP_InputField inputField; // Reference to the TMP input field
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/altarText.txt";

        // Load saved text if it exists
        if (File.Exists(saveFilePath))
        {
            string savedText = File.ReadAllText(saveFilePath);
            inputField.text = savedText;
        }

        inputCanvas.SetActive(false); // Hide the canvas at start
    }

    private void Update()
    {
        // Check if the Escape key is pressed and the canvas is active
        if (inputCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCanvas();
        }
    }

    public void OpenCanvas()
    {
        inputCanvas.SetActive(true); // Show the canvas
        Time.timeScale = 0f; // Pause the game
        inputField.ActivateInputField(); // Focus on the input field
    }

    public void CloseCanvas()
    {
        SaveText();
        inputCanvas.SetActive(false); // Hide the canvas
        Time.timeScale = 1f; // Resume the game
    }

    public void SaveText()
    {
        string textToSave = inputField.text;
        File.WriteAllText(saveFilePath, textToSave);
    }
}
