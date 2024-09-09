using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    private PlayerController _playerControls;
    private CharacterController _characterController;
    private Animator _animator;
    private GameObject _mainCamera;
    private CameraManager _cameraManager;

    #region Controller Input Actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;
    #endregion

    #region Player Movement Settings
    [Header("Player Movement Settings")]
    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _walkSpeed = 2.5f;
    [SerializeField] private float _sprintSpeed = 4.0f;
    [SerializeField] private float _crouchSpeed = 1.5f;
    [SerializeField] private float _movementSpeedSmoothing = 3f;
    [SerializeField] private float _turnSpeedSmoothing = 30f;
    private const float _gravityConstant = -9.81f;
    private const float _rotationSpeed = 180.0f;
    #endregion

    #region Player Movement States
    private bool _isCrouching = false;
    private bool _isRunning = false;
    private bool _isTargeting = false;
    private bool _isPlayerInMotion = false;
    #endregion

    #region Player Movement Variables
    private float _moveSpeed;
    private Vector2 _moveDirection;
    private Vector3 _playerVelocity;
    #endregion

    void Awake()
    {
        _playerControls = new PlayerController();
        _characterController = GetComponent<CharacterController>();

        GameObject cameraGroup = GameObject.FindGameObjectsWithTag("CameraGroup")[0];
        _mainCamera = cameraGroup.transform.Find("MainCamera").gameObject;
        _cameraManager = cameraGroup.GetComponent<CameraManager>();

        GameObject avatar = transform.Find("Avatar").gameObject;
        _animator = avatar.GetComponent<Animator>();

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
        _sprintAction.performed += SprintAction;
        _sprintAction.canceled += SprintAction;

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

    void Update()
    {
        getInputConditions();
        ApplyAnimation();
        ApplyMotion();

        // CheckConditions();
        // ApplyMovement();
        // PlayerJump();
        //Debug.Log("Vertical Velocity: " + _playerVelocity.y);
    }

    private void getInputConditions()
    {
        const float RUNNING_ANIM_FACTOR = 1.5f;

        // Smooth out the movement input to simulate acceleration
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _moveDirection = Vector2.MoveTowards(_moveDirection, moveInput, _movementSpeedSmoothing * Time.deltaTime);

        _isRunning = _sprintAction.ReadValue<float>() > 0;

        // Check if player is running, if so set the position of the running animation
        if (_isRunning)
        {
            _moveDirection.y = RUNNING_ANIM_FACTOR;
        }
    }

    private void ApplyAnimation()
    {
        // Set the speed of the player
        _animator.SetFloat("Speed", _moveDirection.magnitude);
    }

    private void ApplyMotion()
    {
        // Apply gravity
        int isGrounded = _characterController.isGrounded ? 0 : 1;
        _playerVelocity.y += _gravityConstant * Time.deltaTime * isGrounded;

        // Set player speed
        if (_isRunning)
        {
            _moveSpeed = _sprintSpeed;
        }
        else if (_isCrouching)
        {
            _moveSpeed = _crouchSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }

        Vector3 playerRotation = RotatePlayer();
        _playerVelocity.x = playerRotation.x * _moveSpeed * _moveDirection.magnitude;
        _playerVelocity.z = playerRotation.z * _moveSpeed * _moveDirection.magnitude;

        // Apply the movement to the player
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private Vector3 RotatePlayer()
    {
        // Rotate the Avatar to face the direction of movement, according to the view from the camera
        Vector3 moveDirection = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        Vector3 moveDirectionWorld = _mainCamera.transform.TransformDirection(moveDirection);
        moveDirectionWorld.y = 0;
        moveDirectionWorld.Normalize();
        Debug.DrawRay(transform.position, moveDirectionWorld, Color.red);

        // Rotate the avatar to face the direction of movement (making sure the rotation is kept even if the player is not moving)
        if (moveDirectionWorld != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirectionWorld), _turnSpeedSmoothing * Time.deltaTime);
        }

        return moveDirectionWorld;
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
        Vector3 movement = forwardMovement + sideMovement;
        _characterController.Move(movement);

        // HandleRotation();
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
        if (_isPlayerInMotion)
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
        if (_isRunning && _isPlayerInMotion)
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
            // CheckDirectionOfWalking();
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

    // private void CheckDirectionOfWalking()
    // {
    //     if (_moveDirection.y > 0.01f)
    //     {
    //         _moveSpeed = _walkSpeed;
    //         _animator.SetBool("isWalking", true);
    //         _animator.SetBool("isWalkingBackwards", false);
    //         _animator.SetBool("isWalkingRight", false);
    //         _animator.SetBool("isWalkingLeft", false);
    //     }
    //     else if (_moveDirection.y < -0.01f)
    //     {
    //         _moveSpeed = _crouchSpeed;
    //         _animator.SetBool("isWalkingBackwards", true);
    //         _animator.SetBool("isWalking", false);
    //         _animator.SetBool("isWalkingRight", false);
    //         _animator.SetBool("isWalkingLeft", false);
    //     }
    //     else if (_moveDirection.x > 0.01f && _moveDirection.y == 0)
    //     {
    //         _moveSpeed = _crouchSpeed;
    //         _animator.SetBool("isWalkingRight", true);
    //         _animator.SetBool("isWalkingLeft", false);
    //         _animator.SetBool("isWalking", false);
    //         _animator.SetBool("isWalkingBackwards", false);
    //     }
    //     else if (_moveDirection.x < -0.01f && _moveDirection.y == 0)
    //     {
    //         _moveSpeed = _crouchSpeed;
    //         _animator.SetBool("isWalkingLeft", true);
    //         _animator.SetBool("isWalkingRight", false);
    //         _animator.SetBool("isWalking", false);
    //         _animator.SetBool("isWalkingBackwards", false);
    //     }
    // }

    private void CheckConditions()
    {
        _isRunning = _sprintAction.ReadValue<float>() > 0;
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
        if (_characterController.isGrounded)
        {
            _playerVelocity.y = 0f;
        }
        else
        {
            _playerVelocity.y += _gravityConstant * Time.deltaTime;
        }
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded && !_isCrouching)
        {
            // Apply initial jump velocity when grounded and not crouching
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityConstant);
            Debug.Log("Jump triggered with velocity: " + _playerVelocity.y);
        }
    }

    private void CrouchAction(InputAction.CallbackContext context)
    {
        _isCrouching = !_isCrouching;
        _animator.SetBool("isCrouching", _isCrouching);
    }

    private void SprintAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _cameraManager.setFOV("run");
            _cameraManager.setNoise(0.5f, 0.5f);
        }
        else if (context.canceled)
        {
            _cameraManager.setFOV("default", 1f);
            _cameraManager.setNoise(0f, 0f);
        }
    }
}
