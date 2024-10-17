using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUVButton : VRButton
{
    //[SerializeField] GameObject feedManager;
    [SerializeField] GameObject UVSource;
    private UV_source m_uvSrc;
    feed_script feedS;

    public override void Awake()
    {
        base.Awake();
        if (!UVSource.TryGetComponent<UV_source>(out m_uvSrc))
            Debug.LogError("No UV Source component found.", UVSource);
    }
    public override void OnVRTriggerDown(float pressure)
    {
        // TODO: Move hand check to base class and check usingDebug there.
        // TODO: Make this button toggle.
        if (hand != null || VRControllerDebug.usingDebug)
        {
            base.OnVRTriggerDown(pressure);
            m_uvSrc.EnableUV();
        }
    }

	public override void OnVRTriggerUp(float pressure)
	{
        if (hand != null || VRControllerDebug.usingDebug)
        {
		    base.OnVRTriggerUp(pressure);
            m_uvSrc.DisableUV();
        }
	}
}
