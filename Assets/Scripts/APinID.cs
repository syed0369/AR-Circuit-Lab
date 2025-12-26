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
    public int arduinoNodeID; 

    void Awake()
    {
        // Give each Arduino pin a unique high-range ID (e.g., 1000+) 
        // to ensure they never clash with breadboard row IDs.
        arduinoNodeID = 1000 + gameObject.GetInstanceID() % 1000;
    }
}