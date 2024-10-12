using UnityEngine;
using System.Collections.Generic;

public class HandleLight : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    private GameObject _lantern = default;
    private GameObject _player = default;
    private GameObject _rightHand = default;

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
        _lantern = transform.GetChild(0).gameObject; 
        _rightHand = this.transform.parent.gameObject;
        _lantern.SetActive(false); // Start with lantern off
        _player = GameObject.FindGameObjectWithTag("Player");
        _lantern.transform.position = _rightHand.transform.position - new Vector3(0.0f, 0.25f, 0.0f);
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
            _lantern.SetActive(!_lantern.activeSelf);
        }
        else
        {
            Debug.Log("You don't have the lantern in your inventory");
        }
    }
}
