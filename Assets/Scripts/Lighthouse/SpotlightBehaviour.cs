using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehaviour : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10;
    private Light _pointLight;

    void Awake()
    {
        _pointLight = transform.GetComponentInChildren<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        // Rotate the spotlight
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }
}
