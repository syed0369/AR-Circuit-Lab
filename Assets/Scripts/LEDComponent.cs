using UnityEngine;

public class LEDComponent : TwoPinComponentBase
{
    [Header("LED Settings")]
    [Tooltip("Informational only. Anode is physically on the RIGHT in the model.")]
    public bool anodeIsOnRight = true;
}
