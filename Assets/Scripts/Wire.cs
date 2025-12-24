// using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
// public class Wire : MonoBehaviour
// {
//     public BPinID startPin;
//     public BPinID endPin;

//     LineRenderer lr;
//     Camera cam;

//     void Awake()
//     {
//         lr = GetComponent<LineRenderer>();
//         lr.positionCount = 2;
//         cam = Camera.main;
//     }

//     void Update()
//     {
//         if (startPin == null) return;

//         // Start is always the start pin
//         lr.SetPosition(0, startPin.transform.position);

//         if (endPin != null)
//         {
//             // Final connection
//             lr.SetPosition(1, endPin.transform.position);
//         }
//         else
//         {
//             // 🔥 FOLLOW MOUSE while dragging
//             Vector3 mousePos = Input.mousePosition;
//             mousePos.z = 0.5f; // distance from camera (IMPORTANT)
//             Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
//             lr.SetPosition(1, worldPos);
//         }
//     }
// }
