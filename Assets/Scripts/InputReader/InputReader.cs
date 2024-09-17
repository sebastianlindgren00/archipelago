using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject
{
    #region Events
    public event UnityAction PickupItemEvent;
    public event UnityAction DropItemEvent;
    public event UnityAction ToggleLanternEvent;
    public event UnityAction InventorySlot1Event;
    public event UnityAction InventorySlot2Event;
    public event UnityAction InventorySlot3Event;
    public event UnityAction InventorySlot4Event;
    #endregion

    // Ref the PlayerController with all bindings
    private PlayerController _playerController;

    #region Input Actions
    private InputAction pickupItemAction;
    private InputAction dropItemAction;
    private InputAction toggleLanternAction;
    private InputAction inventorySlot1Action;
    private InputAction inventorySlot2Action;
    private InputAction inventorySlot3Action;
    private InputAction inventorySlot4Action;
    #endregion

    private void OnEnable()
    {
        _playerController = new PlayerController();

        #region Init the input actions
        pickupItemAction = _playerController.Player.PickupItem;
        dropItemAction = _playerController.Player.DropItem;
        toggleLanternAction = _playerController.Player.ToggleLantern;
        inventorySlot1Action = _playerController.Player.InventorySlot1;
        inventorySlot2Action = _playerController.Player.InventorySlot2;
        inventorySlot3Action = _playerController.Player.InventorySlot3;
        inventorySlot4Action = _playerController.Player.InventorySlot4;
        #endregion

        #region Enable all input actions
        pickupItemAction.Enable();
        pickupItemAction.performed += PickupItem;

        dropItemAction.Enable();
        dropItemAction.performed += DropItem;

        toggleLanternAction.Enable();
        toggleLanternAction.performed += ToggleLantern;

        inventorySlot1Action.Enable();
        inventorySlot1Action.performed += InventorySlot1;

        inventorySlot2Action.Enable();
        inventorySlot2Action.performed += InventorySlot2;

        inventorySlot3Action.Enable();
        inventorySlot3Action.performed += InventorySlot3;

        inventorySlot4Action.Enable();
        inventorySlot4Action.performed += InventorySlot4;
        #endregion
    }

    // Disable all input actions
    private void OnDisable()
    {
        pickupItemAction.performed -= PickupItem;
        pickupItemAction.Disable();

        dropItemAction.performed -= DropItem;
        dropItemAction.Disable();

        toggleLanternAction.performed -= ToggleLantern;
        toggleLanternAction.Disable();

        inventorySlot1Action.performed -= InventorySlot1;
        inventorySlot1Action.Disable();

        inventorySlot2Action.performed -= InventorySlot2;
        inventorySlot2Action.Disable();

        inventorySlot3Action.performed -= InventorySlot3;
        inventorySlot3Action.Disable();

        inventorySlot4Action.performed -= InventorySlot4;
        inventorySlot4Action.Disable();
    }

    #region Context functions that listens to the input from the bindings
    private void PickupItem(InputAction.CallbackContext context) => PickupItemEvent?.Invoke();
    private void DropItem(InputAction.CallbackContext context) => DropItemEvent?.Invoke();
    private void ToggleLantern(InputAction.CallbackContext context) => ToggleLanternEvent?.Invoke();
    private void InventorySlot1(InputAction.CallbackContext context) => InventorySlot1Event?.Invoke();
    private void InventorySlot2(InputAction.CallbackContext context) => InventorySlot2Event?.Invoke();
    private void InventorySlot3(InputAction.CallbackContext context) => InventorySlot3Event?.Invoke();
    private void InventorySlot4(InputAction.CallbackContext context) => InventorySlot4Event?.Invoke();
    #endregion
}
