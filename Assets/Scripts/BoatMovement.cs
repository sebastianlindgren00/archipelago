using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float bobbingSpeed = 0.2f;
    public float bobbingHeight = 0.05f;
    public float bobbingOffset = 0.03f;
    public float rockingStrength = 1.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AnimateBobbing();
    }

    void AnimateBobbing()
    {
        // Animate the boat bobbing up and down
        Vector3 newPosition = transform.position;

        float waveOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        float randomOffset = Mathf.PerlinNoise(Time.time * bobbingSpeed, 0) * bobbingOffset;

        newPosition.y = waveOffset + randomOffset;
        transform.position = newPosition;

        // Animate the boat rotating
        float rotationX = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        float rotationZ = Mathf.Cos(Time.time * bobbingSpeed + 1) * bobbingHeight;
        transform.rotation = Quaternion.Euler(rotationX * rockingStrength, 0, rotationZ);
    }
}
