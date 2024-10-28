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
using System.Runtime.CompilerServices;
using System;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
  [SerializeField] private float _pickupTextMargin = 2.0f; // Margin between the interact text and the object
  [SerializeField] private GameObject _interactTextContainer; // Text to display when the player can interact with an object
  private TextMeshProUGUI _interactObjectText; // The text to display when the player can interact with an object
  private List<GameObject> _objectsInRange;
  private GameObject closestObject;
  private TextInteraction _textInteraction;
  [SerializeField] private InputReader _inputReader = default;
  private bool _firstTimeLantern = true;
  private GameObject _lantern;
  private bool _firstTimeBook = true;
  private bool _lanternPickedup = false;
  private bool _bookPickedup = false;
  [SerializeField] private TextMeshProUGUI _helpText;

  private void Start()
  {
    //_textInteraction = GetComponent<TextInteraction>();
    _lantern = GameObject.FindGameObjectWithTag("Lantern");
    _objectsInRange = new List<GameObject>();

    // Get the pickup text component, which should be the first child of the pickup text container
    _interactObjectText = _interactTextContainer.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    _interactObjectText.text = "";

    _interactTextContainer.gameObject.SetActive(false);
  }

  // private void OnEnable()
  // {
  //   if(closestObject.name == "Window") {
  //     _inputReader.PickupItemEvent += windowInteraction;
  //   } else {
  //     _inputReader.PickupItemEvent += textInteraction;
  //   }
  // }

  // private void OnDisable()
  // {
  //   if(closestObject.name == "Window") {
  //     _inputReader.PickupItemEvent -= windowInteraction;
  //   } else {
  //     _inputReader.PickupItemEvent -= textInteraction;
  //   }
  // }

  public GameObject GetClosestObject() => closestObject;

  private void Update()
  {
    if (_objectsInRange.Count == 0)
      return;

    // Get the closest object to the player
    closestObject = getClosetObject();

    // Ask the player to interact with the object
    showInteractText(closestObject);
  }

  // Check if an item has entered the player's pickup area and ask the player to pick it up.
  private void OnTriggerEnter(Collider other)
  {
    // Check if the object is an interactable item
    if (!other.CompareTag("Interactable") && !other.gameObject.name.Contains("PickupLantern") && !other.gameObject.name.Contains("Book") && other.gameObject.activeSelf)
      return;

    // Add the object to the list of objects in range
    _objectsInRange.Add(other.gameObject);
  }

  // Check if an item has left the player's interact area.
  private void OnTriggerExit(Collider other)
  {
    // Check if the object is a interactable item
    if (!other.CompareTag("Interactable") && !other.gameObject.name.Contains("PickupLantern") && !other.gameObject.name.Contains("Book") && !other.gameObject.activeSelf)
      return;

    // Remove the object from the list of objects in range
    _objectsInRange.Remove(other.gameObject);

    // Hide the interact text if no objects are in range
    if (_objectsInRange.Count == 0)
      _interactTextContainer.SetActive(false);
  }

  /// <summary>
  /// Get the closest object in range of the players interact area. Useful when multiple objects are in range.
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
      _interactObjectText.text = "read " + objectName;
      if(Input.GetKeyDown(KeyCode.E)) {
        ReadText();
      }
    }
    else if (obj.name.Contains("Window")){
      _interactObjectText.text = "press e to speak";
      if(Input.GetKeyDown(KeyCode.E)) {
        InteractWindow();
      }
    }
    else if (obj.name.Contains("PickupLantern"))
    {
      _interactObjectText.text = "press e to pickup";
      if(Input.GetKeyDown(KeyCode.E) && _firstTimeLantern) {
       DisplayHelpText("PickupLantern", obj);
      }
    }
    else if (obj.name.Contains("Book"))
    {
      _interactObjectText.text = "press e to pickup";
      if(Input.GetKeyDown(KeyCode.E) && _firstTimeBook) {
        DisplayHelpText("Book", obj);
      }
    }

    _interactTextContainer.SetActive(true);
  }

    private void DisplayHelpText(string objectName, GameObject obj)
    {
      Debug.Log("Displaying text");
        if(objectName.Contains("Book"))
        {
            _firstTimeBook = false;
            StartCoroutine(FadeInAndOut(_helpText, objectName, obj, 1f, 2f));
        }
        if(objectName.Contains("PickupLantern"))
        {
          Debug.Log("Displaying lantern toggle text");
            _firstTimeLantern = false;
            StartCoroutine(FadeInAndOut(_helpText, objectName, obj, 1f, 2f));
        }
    }

    // private void textInteraction() => _textInteraction.ReadText();

    private void InteractWindow()
  {
      GameObject closestObject = GetComponent<PlayerInteraction>().GetClosestObject();
      if (!closestObject.name.Contains("Window"))
          return;

      // Get text from the object
      AudioClip audioClip = closestObject.GetComponentInChildren<AudioSource>().clip;
      AudioSource audioSource = closestObject.GetComponentInChildren<AudioSource>();
      if (audioSource != null && audioClip != null)
      {
          audioSource.Play();
      }
  }

  private void ReadText()
  {
      GameObject closestObject = GetComponent<PlayerInteraction>().GetClosestObject();
      if (!closestObject.name.Contains("text"))
          return;

      // Get text from the object
      string text = closestObject.GetComponentInChildren<TextMeshPro>().text;
      Debug.Log(text);
  }

  private IEnumerator FadeInAndOut(TextMeshProUGUI textMeshPro, string type, GameObject obj, float fadeInDuration, float fadeOutDuration)
    {
      _interactObjectText.text = "";
        textMeshPro.gameObject.SetActive(true);
        Color originalColor = textMeshPro.color;
        originalColor.a = 0;
        textMeshPro.color = originalColor;
        if(type == "PickupLantern") {
          textMeshPro.text = "press f to toggle it on and off";
          
          _objectsInRange.Remove(obj);
          if (_objectsInRange.Count == 0)
            _interactTextContainer.SetActive(false);
        } 
        else if(type == "Book") {
          textMeshPro.text = "press one to read it and two to close it";
          _objectsInRange.Remove(obj);
          if (_objectsInRange.Count == 0)
            _interactTextContainer.SetActive(false);
        }
        
        // fade
        while (textMeshPro.color.a < 1)
        {
            originalColor.a += Time.deltaTime / fadeInDuration;
            textMeshPro.color = originalColor;
            yield return null;
        }
        
        yield return new WaitForSeconds(5);
        
        while (textMeshPro.color.a > 0)
        {
            originalColor.a -= Time.deltaTime / fadeOutDuration;
            textMeshPro.color = originalColor;
            yield return null;
        }
        
        textMeshPro.gameObject.SetActive(false);
    }
}