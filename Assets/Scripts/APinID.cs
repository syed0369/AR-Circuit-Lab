using UnityEngine;

public class APinID : MonoBehaviour
{
    [Header("Electrical Identity")]
    public string pinName; // e.g., "D13", "GND", "5V"
    
    [Header("Logic Flags")]
    public bool isPowerSource; // True for D0-D13, 5V, 3.3V
    public bool isGround;      // True for GND pins
    
    // We add a NodeID for the Arduino pins as well so the 
    // WireManager can treat them like Breadboard pins.
    [HideInInspector]
    public string nodeId;


    void Awake()
    {
        // Node naming matches breadboard style
        if (string.IsNullOrEmpty(nodeId))
            nodeId = $"ARD_{pinName}";
    }

}