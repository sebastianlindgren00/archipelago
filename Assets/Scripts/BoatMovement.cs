using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    #region Boat Animation Variables
    [Header("Boat Animation Variables")]
    public float BobbingSpeed = 0.2f;
    public float BobbingHeight = 0.05f;
    public float BobbingOffset = 0.03f;
    public float RockingStrength = 1.2f;
    #endregion

    [Header("Boat Movement Variables")]
    public float MaxSpeed = 0.5f;
    public float Acceleration = 2f;
    public float Deceleration = 0.01f;
    public float TurnSpeed = 0.5f;
    private float _currentSpeed = 0;

    private PlayerController _controls;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the controller
        _controls = new PlayerController();
        _controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = InputManager();
        AnimateRotations(moveDirection);
        Movement(moveDirection);
    }

    float InputManager()
    {
        return _controls.Boat.Movement.ReadValue<float>();
    }

    void Movement(float moveDirection)
    {
        // Calculate where the boats front is facing
        Vector3 forward = transform.forward;

        // Accelerate the boat
        if (Mathf.Abs(moveDirection) > 0 && _currentSpeed <= MaxSpeed)
            _currentSpeed += Acceleration * Time.deltaTime;
        else if (_currentSpeed > 0)
            _currentSpeed -= Deceleration * Time.deltaTime;

        Debug.Log(_currentSpeed);

        // Move the boat in the direction it is facing
        transform.position += forward * _currentSpeed;
    }

    void AnimateRotations(float moveDirection)
    {
        // Animate the boat bobbing up and down
        Vector3 newPosition = transform.position;

        float waveOffset = Mathf.Sin(Time.time * BobbingSpeed) * BobbingHeight;
        float randomOffset = Mathf.PerlinNoise(Time.time * BobbingSpeed, 0) * BobbingOffset;

        newPosition.y = waveOffset + randomOffset;
        transform.position = newPosition;

        // Turn the boat
        float currentYRotation = transform.rotation.eulerAngles.y;
        if (currentYRotation > 180)
            currentYRotation -= 360;    // Normalize the rotation to be between -180 and 180

        // float targetRotation = TURN_LIMIT * moveDirection;
        float rotationY = currentYRotation + moveDirection * Time.deltaTime * TurnSpeed;

        // Animate the boat rocking side to side
        float rotationX = Mathf.Sin(Time.time * BobbingSpeed) * BobbingHeight;
        float rotationZ = Mathf.Cos(Time.time * BobbingSpeed) * BobbingHeight;

        transform.rotation = Quaternion.Euler(rotationX * RockingStrength, rotationY, rotationZ * RockingStrength / 10);
    }
}
