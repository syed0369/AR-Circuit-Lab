using UnityEngine;

public class LEDComponent : TwoPinComponentBase
{
    [Header("LED Settings")]
    [Tooltip("Informational only. Anode is physically on the RIGHT in the model.")]
    public bool anodeIsOnRight = true;
    public string ledColor = "Red"; 
    public string arduinoPin = "D13";
    public float delaySeconds = 1f;
    public bool isConfigured = false;
    public MeshRenderer ledRenderer;
}
