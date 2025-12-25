using UnityEngine;

public class Wire3D : MonoBehaviour
{
    [Header("Visual Settings")]
    public float radius = 0.0015f;
    public float liftAmount = 0.002f;

    private BPinID startPin;
    private BPinID endPin;
    private Transform board;

    public void SetStart(BPinID pin)
    {
        startPin = pin;
        board = pin.transform.parent;
        UpdateTransform(pin.transform.position, pin.transform.position);
    }

    public void UpdateEnd(Vector3 worldPos)
    {
        if (startPin == null) return;
        UpdateTransform(startPin.transform.position, worldPos);
    }

    public void SetEnd(BPinID pin)
    {
        endPin = pin;
        UpdateTransform(startPin.transform.position, pin.transform.position);
    }

    void UpdateTransform(Vector3 a, Vector3 b)
    {
        Vector3 dir = b - a;
        float length = dir.magnitude;
        if (length < 0.0001f) return;

        Vector3 mid = (a + b) * 0.5f;

        Vector3 lift = board.up * radius * 0.5f;
        transform.position = mid + lift;


        transform.rotation = Quaternion.LookRotation(
            dir.normalized,   // forward = wire direction
            board.up          // up = breadboard normal
        );

        transform.Rotate(Vector3.right, 90f, Space.Self);

        transform.localScale = new Vector3(radius, length * 0.5f, radius);
    }

}
