using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    #region Boat Animation Variables
    public float bobbingSpeed = 0.2f;
    public float bobbingHeight = 0.05f;
    public float bobbingOffset = 0.03f;
    public float rockingStrength = 1.2f;
    public float turnSpeed = 0.5f;
    [SerializeField] private const float TURN_LIMIT = 10;
    #endregion
    private PlayerController _controls;
    private BoatMovement _boatMovement;

    // Start is called before the first frame update
    void Start()
    {
        _controls = new PlayerController();
        _controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float moveDirection = InputManager();
        AnimateBobbing(moveDirection);
    }

    float InputManager()
    {
        return _controls.Boat.Movement.ReadValue<float>();
    }

    void OnMovement(InputValue value)
    {
        Debug.Log(value.Get<float>());
    }

    void AnimateBobbing(float moveDirection)
    {
        // Animate the boat bobbing up and down
        Vector3 newPosition = transform.position;

        float waveOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        float randomOffset = Mathf.PerlinNoise(Time.time * bobbingSpeed, 0) * bobbingOffset;

        newPosition.y = waveOffset + randomOffset;
        transform.position = newPosition;

        // Turn the boat
        float currentYRotation = transform.rotation.eulerAngles.y;
        if (currentYRotation > 180)
            currentYRotation -= 360;    // Normalize the rotation to be between -180 and 180

        // Logic to handle the rotation of the boat based on the input (turning radius between 10 and 350 degrees)
        float targetRotation = TURN_LIMIT * moveDirection;
        float rotationY = Mathf.Lerp(currentYRotation, targetRotation, Time.deltaTime * turnSpeed);

        // Animate the boat rotating
        float rotationX = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        float rotationZ = Mathf.Cos(Time.time * bobbingSpeed) * bobbingHeight;

        // Offset the x rotation based on the movement direction
        float currentXRotation = transform.rotation.eulerAngles.x + rotationX;
        if (currentXRotation > 180)
            currentXRotation -= 360;

        float offsetX = 0;
        // offsetX = Mathf.Lerp(currentXRotation, 10.0f * -moveDirection, Time.deltaTime);

        transform.rotation = Quaternion.Euler(rotationX * rockingStrength + offsetX, rotationY, rotationZ * rockingStrength / 10);
    }
}
