using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMixerButton : VRButton
{
    public GameObject feedManager;
    feed_script feedS;
    impeller_script impel;

    /*
    enum ButtonState { 
        PRESSED, // The button was *just* pressed.
        HELD,   // The button is being held down, but isn't being pressed again.
        RELEASED // The button isn't being held.
    } */

    // BUG: There is currently no logic for handling when the user takes their hand
    // away from the button.

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
        if (hand != null)
        {
            feedS.Impellerbuttonpushed = true;
            impel.impellerbuttonpushed = true;
        }
    }
	public override void OnVRTriggerUp(float pressure)
	{
		base.OnVRTriggerUp(pressure);
        // Check if the button's hand != null.
        if (hand != null)
        {
            feedS.Impellerbuttonpushed = false;
            impel.impellerbuttonpushed = false;
        }
	}
}
