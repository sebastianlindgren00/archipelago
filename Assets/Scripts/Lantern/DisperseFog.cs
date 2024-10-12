using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition; // HDRP

public class DisperseFog : MonoBehaviour
{
    private GameObject _fogContainer = default;
    private GameObject _fog = default;
    private GameObject _lanternContainer = default;
    private GameObject _lantern = default;

    void Start()
    { 
        _fogContainer = GameObject.FindGameObjectWithTag("FogContainer");
        _fog = _fogContainer.transform.GetChild(0).gameObject;
        _lanternContainer = GameObject.FindGameObjectWithTag("LanternContainer");
        _lantern = _lanternContainer.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(_fogContainer != null && _lantern != null && hit.gameObject.name == _fogContainer.name && _lantern.activeSelf)
        {
            Disperse();
        }
    }
    

    private IEnumerator DisperseFogCoroutine()
    {
        LocalVolumetricFog fogComponent = _fog.GetComponent<LocalVolumetricFog>();
        float fogDistance = fogComponent.parameters.meanFreePath;

        // Disperse fog
        while(fogDistance < 4.0f)
        {
            fogDistance += 0.2f;
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
