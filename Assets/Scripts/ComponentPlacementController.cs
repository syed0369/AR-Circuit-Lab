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
    public GameObject wirePrefab;
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
        raycaster.HideRadialMenu();
        if(type == ComponentType.LED)
        {
            var led = obj.GetComponent<LEDComponent>();
            
            // find clicked world position from parent pin
            Vector3 pos = parentPin.transform.position;
            Transform board = parentPin.transform.parent;

            LEDSettingsUI.Instance.Open(led, pos, board);
        }

    }

    void HandleWire(BPinID pin)
    {
        if (!WireManager.Instance.HasActiveWire)
        {
            // FIRST PIN SELECTED
            WireManager.Instance.StartWire(wirePrefab, pin);

            // 🔥 Disable Radial Menu ONLY
            raycaster.HideRadialMenu();

            return;
        }

        // SECOND PIN SELECTED
        WireManager.Instance.SelectEndpoint(pin);

        // 🔥 Re-enable menu for next component
        raycaster.EnableMenu();
    }

}
