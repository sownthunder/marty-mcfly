using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Press Q/E to toggle left/right gull-wing doors.
/// Or left-click / tap one of the actual door hitboxes with a camera tagged MainCamera.
/// Works with Legacy Input Manager or new Input System (set Active Input Handling as desired).
/// </summary>
public class ToyTimeMachineInteraction : MonoBehaviour
{
    [Header("Door references")]
    public ToyTimeMachineDoor leftDoor;
    public ToyTimeMachineDoor rightDoor;

    [Header("Mouse / touchscreen hit testing")]
    public Camera interactionCamera;
    public float maximumClickDistance = 200f;

    private void Update()
    {
        bool leftKey = false;
        bool rightKey = false;
        bool click = false;
        Vector2 pointer = Vector2.zero;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            leftKey = keyboard.qKey.wasPressedThisFrame;
            rightKey = keyboard.eKey.wasPressedThisFrame;
        }
        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            click = true;
            pointer = mouse.position.ReadValue();
        }
        Touchscreen touchscreen = Touchscreen.current;
        if (!click && touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
        {
            click = true;
            pointer = touchscreen.primaryTouch.position.ReadValue();
        }
#else
        leftKey = Input.GetKeyDown(KeyCode.Q);
        rightKey = Input.GetKeyDown(KeyCode.E);
        if (Input.GetMouseButtonDown(0))
        {
            click = true;
            pointer = Input.mousePosition;
        }
#endif

        if (leftKey && leftDoor != null) leftDoor.Toggle();
        if (rightKey && rightDoor != null) rightDoor.Toggle();
        if (click) ToggleDoorUnderPointer(pointer);
    }

    public void ToggleLeftDoor()
    {
        if (leftDoor != null) leftDoor.Toggle();
    }

    public void ToggleRightDoor()
    {
        if (rightDoor != null) rightDoor.Toggle();
    }

    private void ToggleDoorUnderPointer(Vector2 screenPosition)
    {
        Camera cam = interactionCamera != null ? interactionCamera : Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0f));
        RaycastHit[] hits = Physics.RaycastAll(ray, maximumClickDistance,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);
        if (hits.Length == 0) return;

        // Respect the nearest hit; clicking through the floor or other obstacles
        // should not interact with a door hidden behind them.
        int nearest = 0;
        for (int i = 1; i < hits.Length; i++)
            if (hits[i].distance < hits[nearest].distance) nearest = i;

        ToyTimeMachineDoor door = hits[nearest].collider.GetComponentInParent<ToyTimeMachineDoor>();
        if (door == leftDoor || door == rightDoor)
            door.Toggle();
    }
}
