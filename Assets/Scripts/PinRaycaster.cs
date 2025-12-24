using UnityEngine;
using UnityEngine.InputSystem;

public class PinRaycaster : MonoBehaviour
{
    public InputAction tapAction;
    public GameObject radialMenuCanvas;

    private Camera cam;
    private BPinID selectedPin; // ALWAYS L or R

    void OnEnable()
    {
        tapAction.Enable();
        tapAction.performed += OnTap;
    }

    void OnDisable()
    {
        tapAction.performed -= OnTap;
        tapAction.Disable();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
        radialMenuCanvas.SetActive(false);
    }

    void OnTap(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos =
            Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed
            ? Touchscreen.current.primaryTouch.position.ReadValue()
            : Pointer.current.position.ReadValue();

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        BPinID pin = hit.transform.GetComponent<BPinID>();
        if (pin == null) return;

        // IMPORTANT: only child pins have colliders
        if (pin.role == BPinRole.Parent) return;

        selectedPin = pin;
        ShowRadialMenu(pin.transform.position);
    }

    void ShowRadialMenu(Vector3 pos)
    {
        radialMenuCanvas.SetActive(true);
        
        // 1. Get the direction 'up' relative to the pin/board, not just world Y
        // This ensures it floats 'above' the surface even if the board is tilted
        Vector3 offsetDirection = selectedPin.transform.up; 
        
        // 2. Increase the offset (try 0.05f or 0.1f if 0.02f is too low)
        float heightOffset = 0.05f; 
        
        radialMenuCanvas.transform.position = pos + (offsetDirection * heightOffset);
        
        // 3. Make it face the camera properly
        radialMenuCanvas.transform.LookAt(radialMenuCanvas.transform.position + cam.transform.forward);
    }

    public BPinID GetSelectedPin() => selectedPin;

    public void ClearSelection()
    {
        selectedPin = null;
        radialMenuCanvas.SetActive(false);
    }
}
