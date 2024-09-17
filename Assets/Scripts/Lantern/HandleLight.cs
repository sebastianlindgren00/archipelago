using UnityEngine;
using System.Collections.Generic;

public class HandleLight : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    private GameObject lantern;
    private bool _lanternOn = false;
    private GameObject _player;
    private List<PickupObject.InventoryItem> inventoryItems;

    private void OnEnable()
    {
        _inputReader.ToggleLanternEvent += ToggleLantern;
    }

    private void OnDisable()
    {
        _inputReader.ToggleLanternEvent -= ToggleLantern;
    }

    void Start()
    {
        lantern = GameObject.FindGameObjectWithTag("Lantern");
        lantern.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        inventoryItems = _player.GetComponent<PickupObject>().inventoryItems;
    }

    private void ToggleLantern()
    {
        Debug.Log("Toggling lantern");
        if (inventoryItems.Count >= 1 && inventoryItems[0].item.name == "PickupLantern")
        {
            _lanternOn = !_lanternOn;
            lantern.SetActive(_lanternOn);
            lantern.GetComponent<Light>().enabled = _lanternOn;
        }
        else
        {
            Debug.Log("You don't have the lantern in your inventory");
        }
    }
}
