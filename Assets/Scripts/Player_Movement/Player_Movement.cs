using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public InputAction playerControls;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private float moveSpeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = playerControls.ReadValue<Vector2>(); 
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed, moveDirection.z * moveSpeed);
    }
}
