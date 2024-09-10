using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Light))]
public class HandleLight : MonoBehaviour
{
    private Light _lanternLight;

    private Vector3 _lanternPos = new Vector3(0, 0, 0);

    PickupObject.inventoryItems _lanternItem; 

    public bool enableFlicker = true;
    private const float MIN_INTENSITY = 1.0f;
    public const float MAX_INTENSITY = 2.0f;
    public const float FLICKER_SPEED = 0.2f;

    void Start()
    {
        _lanternLight = GetComponent<Light>();
        transform.localPosition = _lanternPos;
    }

    void Update()
    {
        // Flicker
        if (enableFlicker && _lanternLight != null)
        {
            _lanternLight.intensity = Mathf.Lerp(MIN_INTENSITY, MAX_INTENSITY, Mathf.PingPong(Time.time * FLICKER_SPEED, 1));
        }
    }

    private void checkIfHeld()
    {

    }
}
