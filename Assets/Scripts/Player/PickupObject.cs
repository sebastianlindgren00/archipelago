using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    [System.Serializable] public struct InventoryItem
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

    private bool _isHeld = false;

    void Awake()
    {
        _playerControls = new PlayerController();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");
        inventoryItems = new List<InventoryItem>(4);
    }

    private void OnEnable()
    {
        _pickupItemAction = _playerControls.Player.PickupItem;
        _pickupItemAction.Enable();
        _pickupItemAction.performed += PickupItem;

        _dropItemAction = _playerControls.Player.DropItem;
        _dropItemAction.Enable();
        _dropItemAction.performed += DropItem;
    }

    private void OnDisable()
    {
        _pickupItemAction.Disable();
        _dropItemAction.Disable();
    }

    void Update()
    { 
        
    }

    private void PickupItem(InputAction.CallbackContext context)
    {
        Debug.Log("Picking up item");
        foreach (GameObject item in pickupItems)
        {
            if (Vector3.Distance(player.transform.position, item.transform.position) < 1.0f)
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
            if (i.item.activeSelf == false)
            {
                i.item.SetActive(true); 
                i.item.transform.position = player.transform.position + player.transform.forward; 
                inventoryItems.Remove(i);
                break; 
            }
        }
    }
}
