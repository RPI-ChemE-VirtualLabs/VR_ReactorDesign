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

    //[SerializeField]
    //private FluidFlow m_fluidFlow;
    [SerializeField] HotFluidInputTemp hfit;
    [SerializeField] ColdFluidInputTemp cfit;
    [SerializeField] HotFluidOutTemp hfot;
    [SerializeField] ColdFluidOutputTemp cfot;

    [Header("Fluid Flow")]

    [SerializeField]
    private TextMeshPro m_fluidFlowDisplay;
    [SerializeField] HotFluidFlowRate hffr;
    [SerializeField] ColdFluidFlowRate cffr;

    [Header("Mixer Status")]

    public Text statustext;
    public Text UVstatustext;
    public Text feedstatustext;
    public Text runtimeText;
    [SerializeField]
    private TextMeshPro m_mixerStatusDisplay;

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
        DisplayFluidFlow();
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
        txt += "Hot fluid input temp: " + hfit.HotFluidInputTempVal + "\n";
        txt += "Hot fluid output temp: " + hfot.HotFluidOutputVal + "\n";
        txt += "Cold fluid input temp: " + cfit.ColdFluidInputTempVal+ "\n";
        txt += "Cold fluid output temp: " + cfot.ColdFluidOutputVal + "\n";
        m_fluidTempDisplay.text = txt;
    }

    private void DisplayFluidFlow()
    {
        string txt = "";
        txt += "Hot fluid flow rate: " + hffr.HotFluidFlowRateVal + "\n";
        txt += "Feed consumed: " + hffr.HWconsumed + "kg\n";
        txt += "Cold fluid flow rate: " + cffr.ColdFluidFlowRateVal + "\n";
        txt += "Cold fluid consumed: " + cffr.CWconsumed + "\n";
        m_fluidFlowDisplay.text = txt;
    }

    private void DisplayMixerStatus()
    {
        string txt = "";
        txt += "Mixer Status: " + (m_fs.impellerOn ? "On" : "Off") + "\n";
        txt += "UV Status:" + (m_uvSrc.isEnabled ? "On" : "Off") + "\n";
        txt += "Feed flow (m^3/min): " + m_fs.F0 + "\n";
        txt += "Runtime (s): " + m_fs.runtime + "\n";
        txt += "C_A (mol/m^3): " + m_fs.CA + "\n";
        m_mixerStatusDisplay.text = txt;
    }
}
