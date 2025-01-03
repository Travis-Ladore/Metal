using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to your TextMeshPro component
    public float typingSpeed = 0.05f; // Time between each character being displayed
    public AudioClip typingSound; // Sound clip to play for each character
    public AudioSource audioSource; // Audio source to play the sound clip
    public string[] dialogueLines; // Array of dialogue lines
    public KeyCode continueKey = KeyCode.Space; // Key to continue to the next line of dialogue

    private int currentLineIndex = 0; // Keeps track of the current line of dialogue
    private bool isTyping = false; // To prevent skipping lines during typing

    private void Start()
    {
        // Start with the first line of dialogue
        if (dialogueLines.Length > 0)
        {
            StartDialogue();
        }
    }

    private void Update()
    {
        // Listen for the continue button press (space by default)
        if (Input.GetKeyDown(continueKey) && !isTyping)
        {
            // Move to the next line of dialogue
            NextLine();
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
    }

    private void NextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
        }
        else
        {
            // Optional: If there's no more dialogue, you can clear the text or trigger another event
            dialogueText.text = "";
            Debug.Log("End of dialogue");
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true; // Disable button press while typing
        dialogueText.text = ""; // Clear the dialogue text

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Add one character at a time

            if (typingSound != null)
            {
                audioSource.PlayOneShot(typingSound); // Play sound for each character
            }

            yield return new WaitForSeconds(typingSpeed); // Wait for the defined speed before displaying the next character
        }

        isTyping = false; // Enable button press after typing
    }
}
