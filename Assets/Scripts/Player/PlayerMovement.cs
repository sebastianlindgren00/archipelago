using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerControls;
    private CharacterController _characterController;
    private Animator _animator;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;

    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _sprintSpeed = 4.0f;
    [SerializeField] private float _crouchSpeed = 1.5f;

    private bool _isCrouching = false;
    private bool _isHoldingSprint = false;
    private bool _isPlayerInMotion = false;

    private float _moveSpeed;
    private float _initialJumpVelocity;

    private Vector2 _moveDirection;
    private Vector3 _movement;
    private Vector3 _playerVelocity;

    private const float _gravityConstant = -9.81f;
    private const float _rotationSpeed = 180.0f;

    void Awake()
    {
        _playerControls = new PlayerController();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
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

        _crouchAction = _playerControls.Player.Crouch;
        _crouchAction.Enable();
        _crouchAction.performed += CrouchAction;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _sprintAction.Disable();
        _crouchAction.Disable();
    }

    void Start()
    {
        _initialJumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravityConstant);
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    private void PlayerMove()
    {
        CheckConditions();
        ApplyMovement(); 
    }

    private void ApplyMovement()
    {
        _moveDirection = _moveAction.ReadValue<Vector2>();

        // W and S
        Vector3 forwardMovement = transform.forward * _moveDirection.y * _moveSpeed * Time.deltaTime;

        // A and D
        Vector3 sideMovement = Vector3.zero;

        if (Mathf.Abs(_moveDirection.y) < 0.01f && Mathf.Abs(_moveDirection.x) > 0.01f)
        {
            sideMovement = transform.right * _moveDirection.x * _moveSpeed * Time.deltaTime;
        }

        // Side moment is 0 when moving forward/backward or forward/backward combined with turning
        _movement = forwardMovement + sideMovement;
        _characterController.Move(_movement); 

        HandleRotation();
    }

    private void HandleRotation()
    {
        float turnInput = _moveDirection.x;
        float forwardInput = _moveDirection.y;

        // Rotate only when moving forward and turning
        if (Mathf.Abs(forwardInput) > 0.01f && Mathf.Abs(turnInput) > 0.01f)
        {
            float rotationAmount = turnInput * _rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }

    private void CheckIfIdleWhenCrouching()
    {
        if(_isPlayerInMotion)
        {
            _animator.SetBool("isIdle", false);
        }
        else
        {
            _animator.SetBool("isIdle", true);
        }
        _moveSpeed = _crouchSpeed;
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWalkingBackwards", false);
        _animator.SetBool("isWalkingRight", false);
        _animator.SetBool("isWalkingLeft", false);
        _animator.SetBool("isCrouching", true);
    }
    
    private void CheckMovementSpeed()
    {
        _animator.SetBool("isCrouching", false);
        if (_isHoldingSprint && _isPlayerInMotion)
        {
            _moveSpeed = _sprintSpeed;
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingBackwards", false);
            _animator.SetBool("isWalkingRight", false);
            _animator.SetBool("isWalkingLeft", false);
        }
        else if (_isPlayerInMotion)
        {
            _animator.SetBool("isRunning", false);
            CheckDirectionOfWalking();
        }
        else
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingBackwards", false);
            _animator.SetBool("isWalkingRight", false);
            _animator.SetBool("isWalkingLeft", false);
            _animator.SetBool("isIdle", true);
            _moveSpeed = 0f;
        }
    }
    
    private void CheckDirectionOfWalking()
    {
        if (_moveDirection.y > 0.01f)
        {
            _moveSpeed = _walkSpeed;
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isWalkingBackwards", false);
            _animator.SetBool("isWalkingRight", false);
            _animator.SetBool("isWalkingLeft", false);
        }
        else if (_moveDirection.y < -0.01f)
        {
            _moveSpeed = _crouchSpeed;
            _animator.SetBool("isWalkingBackwards", true);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingRight", false);
            _animator.SetBool("isWalkingLeft", false);
        }
        else if (_moveDirection.x > 0.01f && _moveDirection.y == 0)
        {
            _moveSpeed = _crouchSpeed;
            _animator.SetBool("isWalkingRight", true);
            _animator.SetBool("isWalkingLeft", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingBackwards", false);
        }
        else if (_moveDirection.x < -0.01f && _moveDirection.y == 0)
        {
            _moveSpeed = _crouchSpeed;
            _animator.SetBool("isWalkingLeft", true);
            _animator.SetBool("isWalkingRight", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWalkingBackwards", false);
        }
    }

    private void CheckConditions()
    {
        _isHoldingSprint = _sprintAction.ReadValue<float>() > 0;
        _isPlayerInMotion = Mathf.Abs(_moveDirection.x) > 0.01f || Mathf.Abs(_moveDirection.y) > 0.01f;
        
        if (_isCrouching)
        {
            CheckIfIdleWhenCrouching();
        }
        else
        {
            CheckMovementSpeed();
        }
    }

    private void PlayerJump()
    {
        _playerVelocity.y += _gravityConstant * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private void JumpAction(InputAction.CallbackContext context)
    {
        Debug.Log("Jumped");
        if (_characterController.isGrounded && !_isCrouching){
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityConstant);
        }
    }

    private void CrouchAction(InputAction.CallbackContext context)
    {
        _isCrouching = !_isCrouching;
    }
}
