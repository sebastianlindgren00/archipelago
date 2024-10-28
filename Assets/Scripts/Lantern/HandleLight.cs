using UnityEngine;
using System.Collections.Generic;

public class HandleLight : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    private GameObject lantern = default;
    private GameObject _player = default;

    private List<PickupObject.InventoryItem> _inventoryItems;

    private void OnEnable()
    {
        _inputReader.ToggleLanternEvent += ToggleLantern;
    }

    private void OnDisable()
    {
        _inputReader.ToggleLanternEvent -= ToggleLantern;
    }

    private void Start()
    {   
        lantern = transform.GetChild(0).gameObject; 
        lantern.SetActive(false); // Start with lantern off
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_player != null)
        {
            _inventoryItems = _player.GetComponent<PickupObject>().inventoryItems;
        }
    }

    private void ToggleLantern()
    {
        Debug.Log("Toggling lantern & inventory count: " + _inventoryItems.Count);

        if (_inventoryItems.Count > 0 && _inventoryItems[0].item.name == "PickupLantern")
        {
            lantern.SetActive(!lantern.activeSelf);
        }
        else
        {
            Debug.Log("You don't have the lantern in your inventory");
        }
    }
}
