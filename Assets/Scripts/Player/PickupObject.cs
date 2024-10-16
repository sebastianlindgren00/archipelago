using UnityEngine;
using System.Collections.Generic;

public class PickupObject : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public GameObject item;
        public bool isHeld;
    }

    public GameObject[] pickupItems;
    public List<InventoryItem> inventoryItems;
    private GameObject _player = default;

    [SerializeField] private InputReader _inputReader = default;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        pickupItems = GameObject.FindGameObjectsWithTag("PickupItem");
        inventoryItems = new List<InventoryItem>(4); // Initialize 4 inventory slots
    }

    private void OnEnable()
    {
        _inputReader.PickupItemEvent += PickupItem;
        _inputReader.DropItemEvent += DropItem;
        _inputReader.InventorySlot1Event += InventorySlot1;
        _inputReader.InventorySlot2Event += InventorySlot2;
        _inputReader.InventorySlot3Event += InventorySlot3;
        _inputReader.InventorySlot4Event += InventorySlot4;
    }

    private void OnDisable()
    {
        _inputReader.PickupItemEvent -= PickupItem;
        _inputReader.DropItemEvent -= DropItem;
        _inputReader.InventorySlot1Event -= InventorySlot1;
        _inputReader.InventorySlot2Event -= InventorySlot2;
        _inputReader.InventorySlot3Event -= InventorySlot3;
        _inputReader.InventorySlot4Event -= InventorySlot4;
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
                i.item.transform.position = _player.transform.position + _player.transform.up;
            }
            else
            {
                i.item.SetActive(false);
            }
        }
    }

    private void PickupItem()
    {
        Debug.Log("Picking up item");
        foreach (GameObject item in pickupItems)
        {
            if (Vector3.Distance(_player.transform.position, item.transform.position) < 2.0f)
            {
                if (item.activeSelf)
                {
                    inventoryItems.Add(new InventoryItem { item = item, isHeld = false });
                    item.SetActive(false);
                    if(item.name == "PickupLantern")
                    {
                        LightOff(item);
                    }
                    break;
                }
            }
        }
    }

    // Set the light in the lantern to inactive
    private void LightOff(GameObject item)
    {
        item.GetComponentInChildren<Light>().enabled = false;
    }

    private void DropItem()
    {
        Debug.Log("Dropping item");
        foreach (InventoryItem i in inventoryItems)
        {
            if (i.isHeld)
            {
                i.item.transform.position = _player.transform.position + _player.transform.up;
                i.item.SetActive(true);
                i.isHeld = false;
                inventoryItems.Remove(i);
                break;
            }
        }
    }

    #region Inventory slots
    private void InventorySlot1() => SetHeldItem(1);
    private void InventorySlot2() => SetHeldItem(2);
    private void InventorySlot3() => SetHeldItem(3);
    private void InventorySlot4() => SetHeldItem(4);
    #endregion

    private void SetHeldItem(int slot)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            item.isHeld = false;
        }

        if (slot < inventoryItems.Count)
        {
            inventoryItems[slot].item.SetActive(true);
            inventoryItems[slot].isHeld = true;
            checkIfBookIsHeld();
        }
    }

    private void checkIfBookIsHeld()
    {
        foreach (InventoryItem i in inventoryItems)
        {
            if (i.isHeld && i.item.name == "Book")
            {
                // Activate book canvas
                Debug.Log("Book is held" + i.item.name);
                // enable the child of book item 
                i.item.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
