using UnityEngine;
using TMPro;

public class LEDSettingsUI : MonoBehaviour
{
    public static LEDSettingsUI Instance;

    [Header("UI References")]
    public GameObject LEDCanvas;
    public GameObject panel;

    [Header("LED Config Inputs")]
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown pinDropdown;
    public TMP_InputField delayInput;

    [Header("Color Mapping")]
    public Material redMat;
    public Material greenMat;
    public Material yellowMat;

    private LEDComponent targetLED;
    private Camera cam;


    void Awake()
    {
        Instance = this;
        LEDCanvas.SetActive(false);
        cam = Camera.main;
    }

    public void Open(LEDComponent led, Vector3 worldPos, Transform board)
    {
        targetLED = led;

        LEDCanvas.SetActive(true);
        panel.SetActive(true);

        // 1. Position: Offset from the board surface
        Vector3 offsetDirection = board.up;
        float heightOffset = 0.05f; 
        LEDCanvas.transform.position = worldPos + (offsetDirection * heightOffset);

        // 2. Rotation Fix: Face the camera while staying vertically aligned
        // We use the camera's up vector to prevent the horizontal/sideways tilt
        LEDCanvas.transform.rotation = Quaternion.LookRotation(
            LEDCanvas.transform.position - cam.transform.position,
            cam.transform.up 
        );

        // 3. Scale: Keep it consistent with your world-space setup
        LEDCanvas.transform.localScale = Vector3.one * 0.001f;
    }

    public void Apply()
    {
        if (targetLED != null)
        {
            // 1. Save Data
            string selectedColor = colorDropdown.options[colorDropdown.value].text;
            targetLED.ledColor = selectedColor;
            targetLED.arduinoPin = pinDropdown.options[pinDropdown.value].text;
            float.TryParse(delayInput.text, out targetLED.delaySeconds);
            targetLED.isConfigured = true;

            // 2. Change Material
            UpdateLEDMaterial(selectedColor);
        }

        LEDCanvas.SetActive(false);
        panel.SetActive(false);
    }

    private void UpdateLEDMaterial(string colorName)
    {
        if (targetLED.ledRenderer == null) return;

        switch (colorName)
        {
            case "Red": targetLED.ledRenderer.material = redMat; break;
            case "Green": targetLED.ledRenderer.material = greenMat; break;
            case "Yellow": targetLED.ledRenderer.material = yellowMat; break;
        }
    }
}