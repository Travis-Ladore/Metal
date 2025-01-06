using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    Outline outline;
    public string message;

    public UnityEvent oninteraction;
    
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void Interactt()
    {
        oninteraction.Invoke();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled=true;
    }

    
}
