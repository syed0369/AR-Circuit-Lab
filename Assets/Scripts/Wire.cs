using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    public BPinID startPin;
    public BPinID endPin;

    [Header("Visual Tuning")]
    [Tooltip("Small vertical offset to avoid z-fighting with breadboard")]
    public float liftAmount = 0.002f;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();

        // Safety defaults (in case prefab is misconfigured)
        lr.positionCount = 2;
        lr.useWorldSpace = true;
    }

    // Called when first pin is selected
    public void SetStart(BPinID pin)
    {
        startPin = pin;

        Vector3 liftedPos = GetLiftedPinPosition(pin);
        lr.SetPosition(0, liftedPos);
        lr.SetPosition(1, liftedPos);
    }

    // Called while dragging
    public void UpdateEnd(Vector3 worldPos)
    {
        if (startPin == null) return;

        lr.SetPosition(0, GetLiftedPinPosition(startPin));
        lr.SetPosition(1, worldPos);
    }

    // Called when second pin is selected
    public void SetEnd(BPinID pin)
    {
        endPin = pin;

        lr.SetPosition(0, GetLiftedPinPosition(startPin));
        lr.SetPosition(1, GetLiftedPinPosition(pin));
    }

    // --- Helper ---
    Vector3 GetLiftedPinPosition(BPinID pin)
    {
        // Lift along board normal (important for tilted AR boards)
        return pin.transform.position + pin.transform.up * liftAmount;
    }
}
