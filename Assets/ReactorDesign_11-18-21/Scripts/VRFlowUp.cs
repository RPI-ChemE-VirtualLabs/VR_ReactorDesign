using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFlowUp : VRButton
{
    public GameObject feedManager;
    feed_script feedS;

    public override void Awake()
    {
        base.Awake();
        if (feedManager)
            feedS = feedManager.GetComponent<feed_script>();
    }
    public override void OnVRTrigger(float pressure)
    {
        base.OnVRTrigger(pressure);
        feedS.feedupbuttonpushed = buttonActive;
    }
}
