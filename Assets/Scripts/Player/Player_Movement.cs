using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovementAndJump : MonoBehaviour
{
    private PlayerController _playerControls;
    private CharacterController _characterController;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;

    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _sprintSpeed = 4.0f;
    private float _moveSpeed;

    private Vector2 _moveDirection;
    private Vector3 _playerVelocity;

    private float _initialJumpVelocity;
    private const float _gravityConstant = -9.81f;

    void Awake()
    {
        _playerControls = new PlayerController();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _moveAction = _playerControls.Player.Move;
        _moveAction.Enable();

        _jumpAction = _playerControls.Player.Jump;
        _jumpAction.Enable();
        _jumpAction.performed += JumpAction;

        _sprintAction = _playerControls.Player.Sprint;
        _sprintAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _sprintAction.Disable();
    }

    void Start()
    {
        _initialJumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravityConstant);
    }

    void Update()
    {
        Debug.Log("Current move speed: " + _moveSpeed);
        PlayerMove();
        PlayerJump();
    }

    private void PlayerMove()
    {
        if(_sprintAction.ReadValue<float>() > 0)
        {
            _moveSpeed = _sprintSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }
       _moveDirection = _moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(_moveDirection.x * _moveSpeed * Time.deltaTime, 0, _moveDirection.y * _moveSpeed * Time.deltaTime);
        _characterController.Move(movement); 
    }

    private void PlayerJump()
    {
        _playerVelocity.y += _gravityConstant * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded){
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityConstant);
            Debug.Log("Jump action performed.");
        }
    }
}
