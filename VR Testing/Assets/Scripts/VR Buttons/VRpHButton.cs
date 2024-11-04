using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRpHButton : VRButton 
{
    [SerializeField]
    private GameObject m_feedManager;
    private feed_script m_feedS;

    [SerializeField]
    private float m_pHVal = 7f;

    public override void Awake()
    {
        base.Awake();
        try
		{
            if (m_feedManager != null)
                m_feedS = m_feedManager.GetComponent<feed_script>();
		}
        catch(System.Exception e)
		{
            Debug.LogError("Unable to get reference to feedManager.");
		}
    }

	public override void OnVRTriggerDown(float pressure)
	{
        if(hand != null || VRControllerDebug.usingDebug)
        {
		    base.OnVRTriggerDown(pressure);
            m_feedS.pHvalue = m_pHVal;
        }
	}
}
