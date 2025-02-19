using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRKeypadButton : VRButton
{
    public int value;
    [SerializeField] private VRKeypad kp;

    public override void OnVRTriggerDown(float pressure)
    {
        base.OnVRTriggerDown(pressure);
        kp.EnterValue(value);
    }
}
