using UnityEngine;

public enum BPinRole
{
    Parent,   // spans 2 holes (LED, Resistor)
    Left,     // single hole (Wire)
    Right     // single hole (Wire)
}

public class BPinID : MonoBehaviour
{
    [Header("Breadboard Location")]
    public string row;
    public int column;

    [Header("Pin Identity")]
    public string nodeId;

    [Header("Pin Role")]
    public BPinRole role;

    [HideInInspector]
    public BPinID parentPin;   // only for Left / Right
}
