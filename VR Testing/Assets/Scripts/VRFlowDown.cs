using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFlowDown : VRButton
{
    [SerializeField]
    private GameObject m_feedManager;
    private feed_script m_feedS;

    [SerializeField]
    float delta = .1f;

    public override void Awake()
    {
		base.Awake();
        try
        {
            if (m_feedManager != null)
                m_feedS = m_feedManager.GetComponent<feed_script>();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unable to get reference to feedManager.");
        }
    }
    public override void OnVRTriggerDown(float pressure)
    {
        if(hand != null || VRControllerDebug.usingDebug)
		{
            m_feedS.F0set += delta;
		}
    }
}
