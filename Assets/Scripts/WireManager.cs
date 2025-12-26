using UnityEngine;
using UnityEngine.InputSystem;
public class WireManager : MonoBehaviour
{
    public static WireManager Instance;

    private Wire3D activeWire;
    private BPinID firstPin;
    private Camera cam;
    public bool HasActiveWire => activeWire != null;
    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void StartWire(GameObject wirePrefab, BPinID pin)
    {
        firstPin = pin;

        GameObject obj = Instantiate(wirePrefab);
        activeWire = obj.GetComponent<Wire3D>();
        activeWire.SetStart(pin);
    }

    public void SelectEndpoint(BPinID pin)
    {
        if (activeWire == null)
            return;

        if (pin == firstPin)
        {
            CancelWire();
            return;
        }

        // VISUAL wire placement
        activeWire.SetEnd(pin);

        // 🧠 ELECTRICAL CONNECTION LOGIC
        string from = firstPin.nodeId;
        string to   = pin.nodeId;

        CircuitGraph.Instance.Connect(from, to);
        Debug.Log($"📌 Wire connected: {from} ↔ {to}");

        // Reset
        firstPin = null;
        activeWire = null;
    }


    void Update()
    {
        if (activeWire == null || firstPin == null) return;

        Vector2 screenPos;

        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Pointer.current != null)
        {
            screenPos = Pointer.current.position.ReadValue();
        }
        else
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(screenPos);

        // 🔥 PROJECT ONTO BREADBOARD PLANE
        Transform board = firstPin.transform.parent;
        Plane boardPlane = new Plane(board.up, board.position);

        if (boardPlane.Raycast(ray, out float enter))
        {
            Vector3 projectedPoint = ray.GetPoint(enter);
            activeWire.UpdateEnd(projectedPoint);
        }
    }

    public void CancelWire()
    {
        if (activeWire != null)
            Destroy(activeWire.gameObject);

        activeWire = null;
        firstPin = null;
    }


    public void SelectEndpointArduino(APinID aPin)
    {
        if (activeWire == null || firstPin == null) return;

        // Position wire visually
        Vector3 endpoint = aPin.transform.position + (aPin.transform.up * 0.001f);
        activeWire.UpdateEnd(endpoint);

        // 🧠 LOGICAL GRAPH CONNECTION
        string from = firstPin.nodeId;
        string to = $"ARD_{aPin.pinName}";        
        if (CircuitGraph.Instance.IsNodeConflict(from, to))
        {
            Debug.LogError($"❌ Conflict: {from} already has another Arduino pin connected!");
            // Optionally show UI popup
            CancelWire();
            return;
        }
        CircuitGraph.Instance.Connect(from, to);

        Debug.Log($"📌 Wire connected from Breadboard {from} → Arduino {aPin.pinName}");

        activeWire = null;
        firstPin = null;
    }

}
