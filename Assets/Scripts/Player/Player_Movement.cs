using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovementAndJump : MonoBehaviour
{
    private PlayerController _controls;
    private CharacterController _controller;
    private InputAction _jump;
    private InputAction _move;
    
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _moveSpeed = 5.0f;

    private Vector3 _playerVelocity;
    private Vector2 _moveDirection;

    private const float _gravity = -9.81f;
    private float _initialJumpVelocity;

    void Awake()
    {
        _controls = new PlayerController();
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _move = _controls.Player.Movement;
        _move.Enable();

        _jump = _controls.Player.Jump;
        _jump.Enable();
        _jump.performed += Jump;
    }

    private void OnDisable()
    {
        _jump.Disable();
    }

    void Start()
    {
        _initialJumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    private void PlayerMove()
    {
       _moveDirection = _move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(_moveDirection.x * _moveSpeed * Time.deltaTime, 0, _moveDirection.y * _moveSpeed * Time.deltaTime);
        _controller.Move(movement); 
    }

    private void PlayerJump()
    {
        _playerVelocity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (_controller.isGrounded){
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            Debug.Log("Jump action performed.");
        }
    }
}
