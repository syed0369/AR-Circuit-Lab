using UnityEngine;
using System.Collections;
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

    [Header("Electrical Nodes")]
    public string anodeNodeId;
    public string cathodeNodeId;

    [Header("Material Sets")]
    public Material redOff;
    public Material redOn;
    public Material greenOff;
    public Material greenOn;
    public Material yellowOff;
    public Material yellowOn;


    public Material offMat;
    public Material onMat;

    private void Start()
    {
        Debug.Log($"[LED] ledRenderer is: {(ledRenderer == null ? "❌ NULL" : "✔ OK")}");
        StartCoroutine(LEDLoop());
    }

    IEnumerator LEDLoop()
    {
        while (true)
        {
            if (IsPowered())
            {
                ledRenderer.material = onMat;
                yield return new WaitForSeconds(delaySeconds);
            }

            ledRenderer.material = offMat;
            yield return new WaitForSeconds(delaySeconds);
        }
    }


    public void InitializeNodeIDs()
    {
        if (attachedParentPin == null) return;

        // The L and R pins already exist under the parent pin
        Transform pinL = attachedParentPin.transform.Find(attachedParentPin.name + "L");
        Transform pinR = attachedParentPin.transform.Find(attachedParentPin.name + "R");

        if (pinL && pinR)
        {
            string leftID  = pinL.GetComponent<BPinID>().nodeId;
            string rightID = pinR.GetComponent<BPinID>().nodeId;

            // Anode is on RIGHT by default
            anodeNodeId = anodeIsOnRight ? rightID : leftID;
            cathodeNodeId = anodeIsOnRight ? leftID : rightID;

            Debug.Log($"💡 LED Pin IDs Set → Anode: {anodeNodeId}, Cathode: {cathodeNodeId}");
        }
    }
    public bool IsPowered()
    {
        if (!isConfigured)
        {
            Debug.Log("⚠️ LED not configured yet.");
            return false;
        }

        if (string.IsNullOrEmpty(anodeNodeId) || string.IsNullOrEmpty(cathodeNodeId))
        {
            Debug.Log("❌ LED node IDs are missing! (anode/cathode)");
            return false;
        }

        bool anodeHasPower =
            CircuitGraph.Instance.IsConnected(anodeNodeId, $"ARD_{arduinoPin}");
        bool cathodeHasGround =
            CircuitGraph.Instance.IsConnected(cathodeNodeId, "ARD_GND");

        Debug.Log($"🔌 CHECK POWER → anode: {anodeNodeId} → {(anodeHasPower ? "⚡" : "❌")}  || cathode: {cathodeNodeId} → {(cathodeHasGround ? "⏚" : "❌")}");

        return anodeHasPower && cathodeHasGround;
    }


}
