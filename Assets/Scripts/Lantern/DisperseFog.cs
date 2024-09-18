using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition; // HDRP

public class DisperseFog : MonoBehaviour
{
    private GameObject _fogContainer = default;
    private GameObject _fog = default;
    private GameObject _lantern = default;

    void Start()
    {
        _lantern = GameObject.FindGameObjectWithTag("Lantern");
        _fogContainer = GameObject.FindGameObjectWithTag("FogContainer");
        _fog = _fogContainer.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Debug.Log("Collided with: " + hit.gameObject.name);
        if(hit.gameObject.name == "FogContainer" && _lantern.activeSelf)
        {
            Debug.Log("Dispersing fog");
            Disperse();
        }
    }
    

    private IEnumerator DisperseFogCoroutine()
    {
        Debug.Log("Dispersing fog coroutine and fog is " + _fog.name);
        // Access fog distance in local volumetric fog component
        LocalVolumetricFog fogComponent = _fog.GetComponent<LocalVolumetricFog>();
        float fogDistance = fogComponent.parameters.meanFreePath;

        // Disperse fog
        while(fogDistance < 10)
        {
            fogDistance += 0.1f;
            fogComponent.parameters.meanFreePath = fogDistance;
            yield return new WaitForSeconds(0.1f);
        }

        // Disable fog
        _fogContainer.GetComponent<Collider>().enabled = false;
    }

    private void Disperse()
    {
        StartCoroutine(DisperseFogCoroutine());
    }
}
