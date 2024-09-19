using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;

    public PlayerInputActions playerControls;
    private InputAction interact;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        interact =playerControls.Player.Interact;
        interact.Enable();
        
    }
    private void OnDisable()
    {
        
        interact.Disable();
    }

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if(interact.triggered && playerInRange && SelectionManager.Instance.onTarget)
        {
            Debug.Log("item added to inventory");
            Destroy(gameObject);
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

    }





}