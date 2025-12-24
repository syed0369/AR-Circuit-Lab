// using UnityEngine;
// using UnityEngine.InputSystem;

// public class WireDragController : MonoBehaviour
// {
//     public GameObject wirePrefab;

//     Wire currentWire;
//     BPinID startPin;
//     Camera cam;

//     void Start()
//     {
//         cam = Camera.main;
//     }

//     void Update()
//     {
//         if (Pointer.current == null) return;

//         // START DRAG
//         if (Pointer.current.press.wasPressedThisFrame)
//         {
//             TryStartWire();
//         }

//         // DRAG
//         if (Pointer.current.press.isPressed && currentWire != null)
//         {
//             Vector3 pos = GetPointerWorldPos();
//             currentWire.UpdateEnd(pos);
//         }

//         // END DRAG
//         if (Pointer.current.press.wasReleasedThisFrame && currentWire != null)
//         {
//             TryEndWire();
//         }
//     }

//     void TryStartWire()
//     {
//         Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
//         if (!Physics.Raycast(ray, out RaycastHit hit)) return;

//         BPinID pin = hit.transform.GetComponent<BPinID>();
//         if (pin == null) return;

//         startPin = pin;

//         GameObject go = Instantiate(wirePrefab);
//         currentWire = go.GetComponent<Wire>();
//         currentWire.SetStart(pin);
//     }

//     void TryEndWire()
//     {
//         Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
//         if (Physics.Raycast(ray, out RaycastHit hit))
//         {
//             BPinID endPin = hit.transform.GetComponent<BPinID>();
//             if (endPin != null)
//             {
//                 currentWire.SetEnd(endPin);
//                 currentWire = null;
//                 return;
//             }
//         }

//         // Cancel wire if not released on pin
//         Destroy(currentWire.gameObject);
//         currentWire = null;
//     }

//     Vector3 GetPointerWorldPos()
//     {
//         Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
//         return ray.origin + ray.direction * 0.05f;
//     }
// }
