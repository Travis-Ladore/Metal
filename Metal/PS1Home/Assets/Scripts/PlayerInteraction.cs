using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    Interact currentInteractable;
    public PlayerInputActions playerControls;
    private InputAction interact;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    public void OnDisable()
    {
        interact.Disable();
    }
    void Update()
    {
        CheckInteraction();
        if(interact.triggered && currentInteractable!= null)
        {
            currentInteractable.Interactt();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
        if(Physics.Raycast(ray, out hit, playerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interact newInteractable = hit.collider.GetComponent<Interact>();
                if(currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }
                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }   
        }
        else
        {
            DisableCurrentInteractable();
        }
    }
    void SetNewCurrentInteractable(Interact newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
    }
    void DisableCurrentInteractable()
    {
        if(currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
