using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    public GameObject[] pickupItems;
    public GameObject[] inventoryItems;
    public GameObject player;
    private PlayerController _playerControls;

    private InputAction _pickupItemAction;
    private InputAction _dropItemAction;

    void Awake()
    {
        _playerControls = new PlayerController();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");
        inventoryItems = new GameObject[3];
    }

    private void OnEnable()
    {
        _pickupItemAction = _playerControls.Player.PickupItem;
        _pickupItemAction.Enable();

        _dropItemAction = _playerControls.Player.DropItem;
        _dropItemAction.Enable();
    }

    private void OnDisable()
    {
        _pickupItemAction.Disable();
        _dropItemAction.Disable();
    }

    void Update()
    {
        ScanForNearbyItems();    
    }

    private void ScanForNearbyItems()
    {
        // Check if player is near an item
        foreach (GameObject item in pickupItems)
        {
            if (Vector3.Distance(player.transform.position, item.transform.position) < 0.5f)
            {
                if(_pickupItemAction.triggered){
                    _pickupItemAction.performed += PickupItem;
                }
                Debug.Log("Press E to pick up " + item.name);
            }
        }
    }

    private void PickupItem(InputAction.CallbackContext context)
    {
        Debug.Log("Picking up item");
        foreach (GameObject item in pickupItems)
        {
            if(item.activeSelf){
                for(int i = 0; i < inventoryItems.Length; i++)
                {
                    if(inventoryItems[i] == item)
                    {
                        break;
                    }
                    if(inventoryItems[i] == null)
                    {
                        inventoryItems[i] = item;
                        item.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
