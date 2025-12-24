using UnityEngine;

public enum ComponentType
{
    LED,
    Resistor,
    Wire
}

public class ComponentPlacementController : MonoBehaviour
{
    public PinRaycaster raycaster;

    public GameObject ledPrefab;
    public GameObject resistorPrefab;

    // ---- Called from Radial Menu Buttons ----

    public void OnSelectLED()
    {
        HandlePlacement(ComponentType.LED);
    }

    public void OnSelectResistor()
    {
        Debug.Log("Resistor button clicked");

        HandlePlacement(ComponentType.Resistor);
    }

    public void OnSelectWire()
    {
        HandlePlacement(ComponentType.Wire);
    }

    // ---- Core Logic ----

    void HandlePlacement(ComponentType type)
    {
        BPinID clickedPin = raycaster.GetSelectedPin();
        if (clickedPin == null) return;

        if (type == ComponentType.Wire)
        {
            HandleWire(clickedPin);
        }
        else
        {
            HandleTwoPinComponent(type, clickedPin);
        }

        raycaster.ClearSelection();
    }

    void HandleTwoPinComponent(ComponentType type, BPinID clickedPin)
    {
        // clickedPin is ALWAYS Left or Right
        BPinID parentPin = clickedPin.transform.parent.GetComponent<BPinID>();

        if (parentPin == null || parentPin.role != BPinRole.Parent)
        {
            Debug.LogError("Parent pin not found!");
            return;
        }

        GameObject prefab =
            type == ComponentType.LED ? ledPrefab : resistorPrefab;

        GameObject obj = Instantiate(prefab);
        Debug.Log($"Instantiated {obj.name}");
        obj.GetComponent<TwoPinComponentBase>()
        .AttachToParentPin(parentPin);
    }

    void HandleWire(BPinID pin)
    {
        if (pin.role == BPinRole.Parent)
        {
            Debug.Log("Wire must be placed on L or R pin, not Parent");
            return;
        }

        Debug.Log($"Wire endpoint selected: {pin.nodeId}");
        // Later:
        // WireManager.SelectEndpoint(pin);
    }
}
