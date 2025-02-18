using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixerStatus : MonoBehaviour
{
    [SerializeField] GameObject UVSourceObj;
    private UV_source m_uvSrc;

    [SerializeField]
    private GameObject m_feedManager;
    private feed_script m_fs;

    [Header("Fluid Temp")]

    [SerializeField]
    private TextMeshPro m_fluidTempDisplay;

    [SerializeField]
    private FluidFlow m_fluidFlow;

    [Header("UV Status")]

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
        DisplayFluidTemp();
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

    // Display input and output temperatures for hot and cold fluid.
    private void DisplayFluidTemp()
    {
        string txt = "";
        txt += "Hot fluid input temp: " + m_fluidFlow.HotFluidFlowRate + "\n";
        txt += "Hot fluid output temp: " + m_fluidFlow.HotFluidOutputVal + "\n";
        txt += "Cold fluid input temp: " + m_fluidFlow.ColdFluidInputTemp+ "\n";
        txt += "Cold fluid output temp: " + m_fluidFlow.ColdFluidOutputVal+ "\n";
        m_fluidTempDisplay.text = txt;
    }
}
