using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    [SerializeField] private float _trailPointLifetime = 10f;
    private float _timeSinceSpawn = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // 50% chance to flip the shoemark
        if (Random.Range(0, 2) == 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        Destroy(gameObject, _trailPointLifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
