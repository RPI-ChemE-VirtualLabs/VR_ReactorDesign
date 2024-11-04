using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixerStatus : MonoBehaviour
{
    [SerializeField] GameObject UVSourceObj;
    private UV_source m_uvSrc;

    [SerializeField] 
    private GameObject m_feedManager;
    private feed_script m_fs;

    public Text statustext;
    public Text UVstatustext;
    public Text feedstatustext;

	// Start is called before the first frame update
	void Start()
    {
        // Find components.
        if(!m_feedManager|| !m_feedManager.TryGetComponent<feed_script>(out m_fs))
            Debug.LogError("Mixer Status couldn't find the feed script.");

        if (!UVSourceObj ||!UVSourceObj.TryGetComponent<UV_source>(out m_uvSrc))
            Debug.LogError("No UV Source component found.", UVSourceObj);
    }

    // Update is called once per frame
    void Update()
    {
        /* Ternary statements:
         *   fs.Impellerbuttonpushed ? "On" : "Off"
         * is essentially:
         * if(fs.Impellerbuttonpushed)
         *  return "On";
         * else
         *  return "Off";
         */
        statustext.text = "Mixer Status: " + (m_fs.impellerOn ? "On" : "Off");
        UVstatustext.text = "UV Status:" + (m_uvSrc.isEnabled ? "On" : "Off");

        // Feed flow.
        feedstatustext.GetComponent<Text>().text = "Feed flow (m3/min): " + m_fs.F0;
    }
}
