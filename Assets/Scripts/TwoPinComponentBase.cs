using UnityEngine;

public abstract class TwoPinComponentBase : MonoBehaviour
{
    protected BPinID attachedParentPin;

    [Header("Placement Settings")]
    public float liftAmount = 0.004f;
    public float uniformScale = 0.2f;

    [Header("Model Orientation Fix")]
    [Tooltip("Local rotation correction for this model (degrees)")]
    public Vector3 rotationOffsetEuler = Vector3.zero;

    public virtual void AttachToParentPin(BPinID parentPin)
    {
        attachedParentPin = parentPin;
        Transform board = parentPin.transform.parent;

        // 1. Find the L and R child transforms
        Transform pinL = null;
        Transform pinR = null;

        foreach (Transform child in parentPin.transform)
        {
            if (child.name.EndsWith("L")) pinL = child;
            if (child.name.EndsWith("R")) pinR = child;
        }

        if (pinL == null || pinR == null) return;

        // 2. Parenting FIRST to make local math easier
        transform.SetParent(board, false); // Set parent without keeping world position

        // 3. Calculate Midpoint in Local Space
        // This makes it much more stable on a moving breadboard
        Vector3 localPosL = board.InverseTransformPoint(pinL.position);
        Vector3 localPosR = board.InverseTransformPoint(pinR.position);
        Vector3 localCenter = (localPosL + localPosR) * 0.5f;

        // 4. Position: Apply with the lift
        transform.localPosition = localCenter + Vector3.up * liftAmount;

        // 5. Rotation: Align length with the holes
        Vector3 localDir = (localPosR - localPosL).normalized;
        transform.localRotation = Quaternion.LookRotation(localDir, Vector3.up);
        
        // Force the 'right' axis to point at the R hole
        transform.right = parentPin.transform.right;
        transform.up = board.up;

        // 6. Apply Prefab-specific offsets (CRITICAL)
        transform.Rotate(rotationOffsetEuler, Space.Self);

        // 7. Scale: Force a visible size
        transform.localScale = Vector3.one * uniformScale;
    }
}