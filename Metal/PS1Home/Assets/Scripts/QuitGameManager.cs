using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameManager : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quitting the game..."); // For testing in the editor
        Application.Quit(); // Quits the application
    }
}
