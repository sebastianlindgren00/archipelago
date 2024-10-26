using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrystalPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _seaCrystal = default;
    [SerializeField] private GameObject _shoreCrystal = default;
    [SerializeField] private GameObject _mistCrystal = default;
    [SerializeField] private GameObject _lastCrystal = default;
    [SerializeField] private TextMeshProUGUI _completionMessage = default;
    [SerializeField] private AudioClip _seaCrystalSound = default;
    [SerializeField] private AudioClip _shoreCrystalSound = default;
    [SerializeField] private AudioClip _mistCrystalSound = default;
    [SerializeField] private AudioClip _lastCrystalSound = default;

    private bool _seaCrystalHit = false;
    private bool _shoreCrystalHit = false;
    private bool _mistCrystalHit = false;
    private bool _lastCrystalHit = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.name == _seaCrystal.name)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if all other bools are false, set this one to true
                if(!_shoreCrystalHit && !_mistCrystalHit && !_lastCrystalHit){
                    _seaCrystalHit = true;
                    CheckCrystalHitOrder();
                }
            }
        }
        else if(hit.gameObject.name == _shoreCrystal.name)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if seaCrystalHit is true and all other bools are false, set this one to true
                if(_seaCrystalHit && !_mistCrystalHit && !_lastCrystalHit){
                    _shoreCrystalHit = true;
                    CheckCrystalHitOrder();
                }
            }
        }
        else if(hit.gameObject.name == _mistCrystal.name)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if seaCrystalHit and shoreCrystalHit are true and lastCrystalHit is false, set this one to true
                if(_seaCrystalHit && _shoreCrystalHit && !_lastCrystalHit){
                    _mistCrystalHit = true;
                    CheckCrystalHitOrder();
                }
            }
        }
        else if(hit.gameObject.name == _lastCrystal.name)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // if all other bools are true, set this one to true
                if(_seaCrystalHit && _shoreCrystalHit && _mistCrystalHit){
                    _lastCrystalHit = true;
                    CheckCrystalHitOrder();
                }
            }
        }
    }

    private void CheckCrystalHitOrder()
    {
        if (_seaCrystalHit && _shoreCrystalHit && _mistCrystalHit && _lastCrystalHit)
        {
            Debug.Log("Crystals hit in the correct order!");
            _lastCrystal.GetComponent<AudioSource>().PlayOneShot(_lastCrystalSound);
            StartCoroutine(FadeInAndOut(_completionMessage, 1f, 2f));
        }
        else if (_seaCrystalHit && !_shoreCrystalHit && !_mistCrystalHit && !_lastCrystalHit)
        {
            Debug.Log("Sea crystal hit first.");
            _seaCrystal.GetComponent<AudioSource>().PlayOneShot(_seaCrystalSound);
        }
        else if (_seaCrystalHit && _shoreCrystalHit && !_mistCrystalHit && !_lastCrystalHit)
        {
            Debug.Log("Shore crystal hit second.");
            _shoreCrystal.GetComponent<AudioSource>().PlayOneShot(_shoreCrystalSound);
        }
        else if (_seaCrystalHit && _shoreCrystalHit && _mistCrystalHit && !_lastCrystalHit)
        {
            Debug.Log("Mist crystal hit third.");
            _mistCrystal.GetComponent<AudioSource>().PlayOneShot(_mistCrystalSound);
        }
        else
        {
            Debug.Log("Crystals hit in the wrong order. Resetting...");
            _seaCrystalHit = false;
            _shoreCrystalHit = false;
            _mistCrystalHit = false;
            _lastCrystalHit = false;
        }
        
    }

    private IEnumerator FadeInAndOut(TextMeshProUGUI textMeshPro, float fadeInDuration, float fadeOutDuration)
    {
        textMeshPro.gameObject.SetActive(true);
        Color originalColor = textMeshPro.color;
        originalColor.a = 0;
        textMeshPro.color = originalColor;
        
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
