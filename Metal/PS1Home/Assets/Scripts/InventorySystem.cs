using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction tab;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
   
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;




    public bool isOpen;

    //public bool isFull;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        

        PopulateSlotList();
    }


    void Update()
    {

        if (tab.triggered && !isOpen)
        {

            Debug.Log("tab is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;

        }
        else if (tab.triggered && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
       whatSlotToEquip = FindNextEmptySlot();

       itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
       itemToAdd.transform.SetParent(whatSlotToEquip.transform);

    }

    private void OnEnable()
    {
        tab = playerControls.Player.Inventory;
        tab.Enable();
    }
    private void OnDisable()
    {
        tab.Disable();
    }

    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    public GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
                
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter += 1;
            }
        
        }
        if(counter == 10)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}