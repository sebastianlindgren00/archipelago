/// ===========================
// Description:
//   A script to handle the player's interaction mechanic
// ---------------------------
// Code borrowed from:
//   -
// ---------------------------
// Created by Albin Kjellberg
// ===========================

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
  [SerializeField] private float _pickupTextMargin = 2.0f; // Margin between the interact text and the object
  [SerializeField] private GameObject _interactTextContainer; // Text to display when the player can interact with an object
  private TextMeshProUGUI _interactObjectText; // The text to display when the player can interact with an object
  private List<GameObject> _objectsInRange;
  private GameObject closestObject;
  private TextInteraction _textInteraction;
  [SerializeField] private InputReader _inputReader = default;

  private void Start()
  {
    _textInteraction = GetComponent<TextInteraction>();

    _objectsInRange = new List<GameObject>();

    // Get the pickup text component, which should be the first child of the pickup text container
    _interactObjectText = _interactTextContainer.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    _interactObjectText.text = "";

    _interactTextContainer.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    _inputReader.PickupItemEvent += textInteraction;
  }

  private void OnDisable()
  {
    _inputReader.PickupItemEvent -= textInteraction;
  }

  public GameObject GetClosestObject() => closestObject;

  private void Update()
  {
    if (_objectsInRange.Count == 0)
      return;

    // Get the closest object to the player
    closestObject = getClosetObject();

    // Ask the player to pick up the object
    showInteractText(closestObject);
  }

  // Check if an item has entered the player's pickup area and ask the player to pick it up.
  private void OnTriggerEnter(Collider other)
  {
    // Check if the object is a pickup item
    if (!other.CompareTag("Interactable"))
      return;

    // Add the object to the list of objects in range
    _objectsInRange.Add(other.gameObject);
  }

  // Check if an item has left the player's pickup area.
  private void OnTriggerExit(Collider other)
  {
    // Check if the object is a pickup item
    if (!other.CompareTag("Interactable"))
      return;

    // Remove the object from the list of objects in range
    _objectsInRange.Remove(other.gameObject);

    // Hide the pickup text if no objects are in range
    if (_objectsInRange.Count == 0)
      _interactTextContainer.SetActive(false);
  }

  /// <summary>
  /// Get the closest object in range of the players pickup area. Useful when multiple objects are in range.
  /// </summary>
  /// <returns>The closest object in range</returns>
  private GameObject getClosetObject()
  {
    GameObject closestObject = null;
    float closestDistance = float.MaxValue;

    // Check which object is closest to the player
    foreach (GameObject obj in _objectsInRange)
    {
      float distance = Vector3.Distance(obj.transform.position, transform.position);
      if (distance < closestDistance)
      {
        closestObject = obj;
        closestDistance = distance;
      }
    }

    return closestObject;
  }

  private void showInteractText(GameObject obj)
  {
    // Get the objects position in screen space
    Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

    // Set the text position to the objects position
    _interactTextContainer.transform.position = screenPos + Vector3.up * _pickupTextMargin;
    string objectName = obj.name.Split('_')[0];

    // Set the text to the objects name
    if (obj.name.Contains("text"))
    {
      _interactObjectText.text = "Read " + objectName;

    }
    else
      _interactObjectText.text = obj.name;

    _interactTextContainer.SetActive(true);
  }

  private void textInteraction() => _textInteraction.ReadText();
}