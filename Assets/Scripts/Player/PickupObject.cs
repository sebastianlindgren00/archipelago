using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [System.Serializable] 
    public class InventoryItem // Changed from struct to class
    {
        public GameObject item;
        public bool isHeld;
    }

    public GameObject[] pickupItems;
    public List<InventoryItem> inventoryItems;
    public GameObject player;

    private PlayerController _playerControls;
    private InputAction _pickupItemAction;
    private InputAction _dropItemAction;
    private InputAction _inventorySlot1;
    private InputAction _inventorySlot2;
    private InputAction _inventorySlot3;
    private InputAction _inventorySlot4;

    void Awake()
    {
        _playerControls = new PlayerController();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");
        inventoryItems = new List<InventoryItem>(4); // Initialize 3 inventory slots
    }

    private void OnEnable()
    {
        _pickupItemAction = _playerControls.Player.PickupItem;
        _pickupItemAction.Enable();
        _pickupItemAction.performed += PickupItem;

        _dropItemAction = _playerControls.Player.DropItem;
        _dropItemAction.Enable();
        _dropItemAction.performed += DropItem;

        _inventorySlot1 = _playerControls.Player.InventorySlot1;
        _inventorySlot1.Enable();
        _inventorySlot1.performed += InventorySlot1;

        _inventorySlot2 = _playerControls.Player.InventorySlot2;
        _inventorySlot2.Enable();
        _inventorySlot2.performed += InventorySlot2;

        _inventorySlot3 = _playerControls.Player.InventorySlot3;
        _inventorySlot3.Enable();
        _inventorySlot3.performed += InventorySlot3;

        _inventorySlot4 = _playerControls.Player.InventorySlot4;
        _inventorySlot4.Enable();
        _inventorySlot4.performed += InventorySlot4;
    }

    private void OnDisable()
    {
        _pickupItemAction.Disable();
        _dropItemAction.Disable();
        _inventorySlot1.Disable();
        _inventorySlot2.Disable();
        _inventorySlot3.Disable();
        _inventorySlot4.Disable();
    }

    void Update()
    { 
        IsItemHeld();
    }

    private void IsItemHeld()
    {
        foreach (InventoryItem i in inventoryItems)
        {
            if (i.isHeld)
            {
                i.item.transform.position = player.transform.position + player.transform.up;
            }
            else
            {
                i.item.SetActive(false);
            }
        }
    }

    private void PickupItem(InputAction.CallbackContext context)
    {
        Debug.Log("Picking up item");
        foreach (GameObject item in pickupItems)
        {
            if (Vector3.Distance(player.transform.position, item.transform.position) < 2.0f)
            {
                if (item.activeSelf)
                {
                    inventoryItems.Add(new InventoryItem { item = item, isHeld = false });
                    item.SetActive(false); 
                    break; 
                }
            }
        }
    }

    private void DropItem(InputAction.CallbackContext context)
    {
        Debug.Log("Dropping item");
        foreach (InventoryItem i in inventoryItems)
        {
            if (i.isHeld)
            {
                i.item.transform.position = player.transform.position + player.transform.forward;
                i.item.SetActive(true);
                i.isHeld = false;
                inventoryItems.Remove(i);
                break;
            }
        }
    }

    private void InventorySlot1(InputAction.CallbackContext context)
    {
        inventoryItems[0].item.SetActive(true);

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.isHeld)
            {
                item.isHeld = false;
            }
        }

        inventoryItems[0].isHeld = true;
    }

    private void InventorySlot2(InputAction.CallbackContext context)
    {
        inventoryItems[1].item.SetActive(true);

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.isHeld)
            {
                item.isHeld = false;
            }
        }

        inventoryItems[1].isHeld = true;
    }

    private void InventorySlot3(InputAction.CallbackContext context)
    {
        inventoryItems[2].item.SetActive(true);

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.isHeld)
            {
                item.isHeld = false;
            }
        }

        inventoryItems[2].isHeld = true;
    }

    private void InventorySlot4(InputAction.CallbackContext context)
    {
        inventoryItems[3].item.SetActive(true);

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.isHeld)
            {
                item.isHeld = false;
            }
        }

        inventoryItems[3].isHeld = true;
    }
}
