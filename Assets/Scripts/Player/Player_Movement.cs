using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementAndJump : MonoBehaviour
{
    // Variables for movement
    private PlayerController _controls;
    private Vector2 _moveDirection = Vector2.zero;
    private CharacterController _controller;
    [SerializeField] private float _moveSpeed = 5.0f;

    // Variables for jump and gravity
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    [SerializeField] private float _jumpHeight = 2.0f;
    private float _gravityValue = -9.81f;
    private InputAction _jumpAction;
    private float _initialJumpVelocity;

    void Awake()
    {
        // Initialize Input System
        _controls = new PlayerController();
        _jumpAction = _controls.Player.Jump; // Bind the jump action
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _jumpAction.performed += OnJumpPerformed; // Register jump action
    }

    private void OnDisable()
    {
        _jumpAction.performed -= OnJumpPerformed; // Unregister jump action
        _controls.Player.Disable();
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        // Calculate initial jump velocity based on height
        _initialJumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
    }

    void Update()
    {
        // Check if the player is grounded
        _isGrounded = _controller.isGrounded;

        // Reset vertical velocity when grounded
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;  // Keep player grounded
        }

        // Handle movement
        MovePlayer();

        // Apply gravity
        if (!_isGrounded)
        {
            _playerVelocity.y += _gravityValue * Time.deltaTime;  // Simulate gravity
        }

        // Apply vertical movement (jump and gravity)
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    void MovePlayer()
    {
        // Read movement input from the Input System
        _moveDirection = _controls.Player.Movement.ReadValue<Vector2>();

        // Convert the 2D movement direction to 3D (x and z axes)
        Vector3 move = new Vector3(_moveDirection.x, 0.0f, _moveDirection.y);

        // Apply movement on the horizontal plane (x and z)
        _controller.Move(move * _moveSpeed * Time.deltaTime);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Perform jump if the player is grounded
        if (_isGrounded)
        {
            Debug.Log("Jump action performed.");
            Jump();
        }
    }

    void Jump()
    {
        // Apply the initial jump velocity when jumping
        _playerVelocity.y = _initialJumpVelocity;
        Debug.Log("Jumping! Initial Velocity Y: " + _playerVelocity.y);
    }
}
