using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition; // HDRP

public class DisperseFog : MonoBehaviour
{
    private LocalVolumetricFog _fog;
    private PickupObject _pickupObject;
    private GameObject test;

    void Start()
    {
        // _fog = GetComponent<LocalVolumetricFog>();
        // test = _pickupObject.lantern;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
