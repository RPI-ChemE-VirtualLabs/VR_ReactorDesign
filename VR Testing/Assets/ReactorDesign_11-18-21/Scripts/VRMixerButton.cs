using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMixerButton : VRButton
{
    public GameObject feedManager;
    feed_script feedS;
    impeller_script impel;

    public override void Awake()
    {
        base.Awake();
        if (feedManager)
            feedS = feedManager.GetComponent<feed_script>();

        // Find reference to impeller shaft.
        GameObject impObj = GameObject.Find("Impeller_script");
        impel = impObj.GetComponent<impeller_script>();
    }
    public override void OnVRTriggerDown(float pressure)
    {
        base.OnVRTriggerDown(pressure);
        feedS.Impellerbuttonpushed = true;
        impel.impellerbuttonpushed = true;
    }
	public override void OnVRTriggerUp(float pressure)
	{
		base.OnVRTriggerUp(pressure);
        feedS.Impellerbuttonpushed = false;
        impel.impellerbuttonpushed = false;
	}
}
