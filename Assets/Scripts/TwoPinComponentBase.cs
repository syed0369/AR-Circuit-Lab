using UnityEngine;

public abstract class TwoPinComponentBase : MonoBehaviour
{
    protected BPinID attachedParentPin;

    [Header("Placement Settings")]
    public float liftAmount = 0.004f;
    public float uniformScale = 0.2f;

    [Header("Model Orientation Fix")]
    [Tooltip("Adjust these if the model is rotated incorrectly relative to the pins")]
    public Vector3 rotationOffsetEuler = Vector3.zero;

    public virtual void AttachToParentPin(BPinID parentPin)
    {
        attachedParentPin = parentPin;
        // Assume the parent of the pin is the Board/Breadboard
        Transform board = parentPin.transform.parent;

        // 1. Find the L and R child transforms
        Transform pinL = null;
        Transform pinR = null;

        foreach (Transform child in parentPin.transform)
        {
            if (child.name.EndsWith("L")) pinL = child;
            else if (child.name.EndsWith("R")) pinR = child;
        }

        if (pinL == null || pinR == null)
        {
            Debug.LogWarning($"Pins L and R not found on {parentPin.name}");
            return;
        }

        // 2. Parenting
        // Parenting to the board ensures the component moves/rotates with the board
        transform.SetParent(board, true);

        // 3. Calculate Midpoint and Direction
        Vector3 posL = pinL.position;
        Vector3 posR = pinR.position;
        Vector3 centerPoint = (posL + posR) * 0.5f;
        Vector3 directionToRightPin = (posR - posL).normalized;

        // 4. Position: Apply position with the lift (offset along the board's Up axis)
        transform.position = centerPoint + (board.up * liftAmount);

        // 5. Rotation: 
        // We want the component's 'Forward' or 'Right' to align with the pins.
        // LookRotation(forward, up) aligns the Z-axis to the pins. 
        // If your model's length is along the X-axis, we use Right instead.
        Quaternion targetRotation = Quaternion.LookRotation(directionToRightPin, board.up);
        
        // Apply the calculated rotation
        transform.rotation = targetRotation;

        // 6. Apply Model-Specific Correction
        // This handles models that weren't exported with the correct forward axis
        transform.localRotation *= Quaternion.Euler(rotationOffsetEuler);

        // 7. Scale
        transform.localScale = Vector3.one * uniformScale;

        if (TryGetComponent<LEDComponent>(out LEDComponent led))
        {
            led.InitializeNodeIDs();
        }

    }
}