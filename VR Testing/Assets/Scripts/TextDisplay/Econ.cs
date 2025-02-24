using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Econ : MonoBehaviour
{
    //public float watercost =0f;
    public float NaOHcost = 0f;
    public double ColdFluidCost = 0f;
    public float NaOHconsumed = 0f; //kg
    public float pHvalue = 7f;
    public double pumpingcost = 0f;
    public double impellercost = 0d;
    public double UVcost = 0d;
    public double electricitycost = 0d;
    public double totalcost = 0d;

    public double ColdFluidFlowRateVal; //kg/s
    public double ColdFluidInputTempVal; //kg/s
    public double HotFluidOutTempVal; //K
    public double HotFluidInputTempVal; //K

    // public float ColdFluidInletTemp; // K
    public double costperkg = (0.056f/1000f); // cost of chilling water per kg

    // GameObject containing the economy window background and canvas./
    [SerializeField] GameObject econViewWindow;

    public GameObject feed_script; // script
    public GameObject pHsettings; // script
    public GameObject ColdFluidFlowRate; // script
    public GameObject HotFluidFlowRate; // script
    public GameObject HotFluidOutTemp; // script
    public GameObject ColdFluidInputTemp;  //script
    public GameObject HotFluidInputTemp;  //script
    public GameObject impeller_script;  //script
    public GameObject ResetEconButton;

    public Material yellow;
    public Material stairprops;

    public Text NaOHcostText;
    public Text ColdFluidCostText;
    public Text DepreciationText;
    public Text LaborcostText;
    public Text electricitycostText;
    public Text totalcostText;
    public Text econtimeText;
    public Text costperminText;

    // Number of seconds between econ entries.
    private float m_exportInterval = 30;
    // Filestream pointing to new file.
    private System.IO.StreamWriter fs;

    public float runtime = 0.00000f;
    public float starttime = 0.00000f;
    public float recordtime = 0.00000f;
    public float deltat; // time interval between last two frames in seconds
    public int nframes = 0; // number of frames elapsed
    private float oldnpoints;
    public float npoints = 0.0f;
    //private int int_npoints = 0;
    private float begintime;
    public float frameselapsed = 0.0f;
    public float timelapsed = 0.0f;
    private float timelapsed_prev;
    public float framerate;
    private float lastframetime;
    public float runtimeprev;
    public float depreciation = 0f;
    public float Laborcost = 0f;

    public float impfactor = 0;
    public float feedfactor = 0;
    public float UVfactor = 0;
    public float F0;
    public float tempval;
    public float NewNaOHconsumed =0;
    public float NaOHconc =0;

    public float econtime =0;
    public float resettime = 0;
    public double costpermin = 0;

    public double CWConsumed;
    public double HWConsumed;

    public bool impellerbuttonpushed = false;
    public bool UVbuttonpushed = false;
    public bool feedbuttonpushed = false;

    // Reference to external economy view object.
    [SerializeField] private GameObject m_econView;
    // Text content.
    [SerializeField] private string m_econContent;

	private void Awake()
	{
        //InitExportEcon();
        //InvokeRepeating("WriteEconLine", 0, 30);
        // Assign window toggle to menu button.
        VR_CharacterController.menuDown += ToggleEconWindow;
        VR_CharacterController.menuDownTriple += ResetEcon;

        //m_econView = GameObject.Find("Econ View Window");
	}

	// Start is called before the first frame update
	void Start()
    {
        begintime += Time.deltaTime;

        pumpingcost = 0f;
        impellercost = 0d;
        UVcost = 0d;
        electricitycost = 0d;
        NaOHcost = 0f;
        totalcost = 0d;
    }

    // Update is called once per frame
    void Update()
    {

        // ***** Timekeeping operations

        frameselapsed = frameselapsed + 1.0f;
        timelapsed_prev = timelapsed;
        timelapsed += Time.deltaTime;
        lastframetime = timelapsed - timelapsed_prev;
        framerate = frameselapsed / timelapsed;
        runtimeprev = runtime;
        runtime += Time.deltaTime; //feed_script.GetComponent<feed_script>().runtime;
        econtime = runtime - resettime;
        ResetEconButton.GetComponent<MeshRenderer>().material = stairprops;

        HotFluidInputTempVal = HotFluidInputTemp.GetComponent<HotFluidInputTemp>().HotFluidInputTempVal;
        HotFluidOutTempVal = HotFluidOutTemp.GetComponent<HotFluidOutTemp>().HotFluidOutputVal;
       
        pHvalue = pHsettings.GetComponent<pHsettings>().pHvalue;

        NaOHconc = pHsettings.GetComponent<pHsettings>().NaOHconc;

        NewNaOHconsumed = NewNaOHconsumed + NaOHconc * 1000f * 0.04f * (runtime - runtimeprev) * (F0 * Time.fixedDeltaTime); 

        NaOHconsumed = pHsettings.GetComponent<pHsettings>().NaOHconsumed; // flowrate in m^3/s from feed_script


        NaOHcost = NewNaOHconsumed*0.45f;  // based on current NaOH price of 45c per kg
        //NaOHcostText.GetComponent<Text>().text = "NaOH Cost ($): " + System.Math.Round(NaOHcost, 3);
        m_econContent = "NaOH Cost ($): " + System.Math.Round(NaOHcost, 3) + "\n";

        impellerbuttonpushed = impeller_script.GetComponent<impeller_script>().impellerbuttonpushed;
        UVbuttonpushed = feed_script.GetComponent<feed_script>().UVbuttonpushed;
        feedbuttonpushed = feed_script.GetComponent<feed_script>().feedOn;

        // Modify factors for process statuses.
        impfactor = impellerbuttonpushed ? 1 : 0;
        feedfactor = feedbuttonpushed ? 1 : 0;
        UVfactor = UVbuttonpushed ? 1 : 0;

        if (pHvalue == 7)
        {
            if (HotFluidOutTempVal < 330.1)
            {
                depreciation = depreciation + 6.19e-5f * (runtime - runtimeprev); // $/s
            }
            else
            {
                tempval = (float)HotFluidOutTempVal;
                depreciation = depreciation + 6.19e-5f * 25f * (runtime - runtimeprev)*Mathf.Exp((tempval-330f)/10f); // $/s
            }
        }
        if (pHvalue == 10)
        {
            if (HotFluidOutTempVal < 330.1)
            {
                depreciation = depreciation + 0.00062f * (runtime - runtimeprev); // $/s
            }
            else
            {
                tempval = (float)HotFluidOutTempVal;
                depreciation = depreciation + 0.00062f * 5f * (runtime - runtimeprev) * Mathf.Exp((tempval - 330f) / 10f); // $/s


            }
        }
        if (pHvalue == 12)
        {
            if (HotFluidOutTempVal < 330.1)
            {
                depreciation = depreciation + 0.00495f * (runtime - runtimeprev); // $/s
            }
            else
            {
                tempval = (float)HotFluidOutTempVal;
                depreciation = depreciation + 0.00495f * (runtime - runtimeprev) * Mathf.Exp((tempval - 330f) / 10f); // $/s
            }    
        }

        // DepreciationText.GetComponent<Text>().text = "Depreciation Cost ($): " + System.Math.Round(depreciation, 3);
        m_econContent += "Depreciation Cost ($): " + System.Math.Round(depreciation, 3) + "\n";

        Laborcost = Laborcost + 0.0019f* (runtime - runtimeprev); //$/s
        //LaborcostText.GetComponent<Text>().text = "Labor Cost ($): " + System.Math.Round(Laborcost, 3);
        m_econContent += "Labor Cost ($): " + System.Math.Round(Laborcost, 3) + "\n";

        ColdFluidFlowRateVal = ColdFluidFlowRate.GetComponent<ColdFluidFlowRate>().ColdFluidFlowRateVal; // in kg/min
        ColdFluidInputTempVal = ColdFluidInputTemp.GetComponent<ColdFluidInputTemp>().ColdFluidInputTempVal;
        costperkg = 0.000056 + ((20 - ColdFluidInputTempVal + 273) * 0.0175)/20000; 

        ColdFluidCost = ColdFluidCost + feedfactor*(lastframetime * costperkg * ColdFluidFlowRateVal/60d);
        //ColdFluidCostText.GetComponent<Text>().text = "Chilling Water Cost ($): " + System.Math.Round(ColdFluidCost, 3);
        m_econContent += "Chilling Water Cost ($): " + System.Math.Round(ColdFluidCost, 3) + "\n";
        
        CWConsumed = ColdFluidFlowRate.GetComponent<ColdFluidFlowRate>().CWconsumed;
        HWConsumed = HotFluidFlowRate.GetComponent<HotFluidFlowRate>().HWconsumed;

        F0 = feed_script.GetComponent<feed_script>().F0;
        pumpingcost = pumpingcost + 0.1785*(F0)*(runtime-runtimeprev)/(3600d) + 0.1785*(ColdFluidFlowRateVal/1000d)*(runtime - runtimeprev)/(3600d); // $/s based on $0.1785 /hr with F0 = 1 m^3/min
        impellercost = impellercost + 0.056d*3.000d * (runtime - runtimeprev) / (3600d); // $/s assumed 3.000 kW power consumption
        UVcost = UVcost + 0.056d*1.000d * (runtime - runtimeprev) / (3600d); // $/s assumed 1.000 kW power consumption
        electricitycost = pumpingcost + impellercost + UVcost;  // $/s

        totalcost = electricitycost + depreciation + Laborcost + NaOHcost + ColdFluidCost;

        //electricitycostText.GetComponent<Text>().text = "Electricity Cost ($): " + System.Math.Round(electricitycost, 3);
        //totalcostText.GetComponent<Text>().text = "Total Cost ($): " + System.Math.Round(totalcost, 3);
        //econtimeText.GetComponent<Text>().text = "Econ. time (s): " + System.Math.Round(econtime, 3);
        m_econContent += "Electricity Cost ($): " + System.Math.Round(electricitycost, 3) + "\n";
        m_econContent += "Total Cost ($): " + System.Math.Round(totalcost, 3) + "\n";
        m_econContent += "Econ. time (s): " + System.Math.Round(econtime, 3) + "\n";

        costpermin = 60*totalcost / econtime; // $ per minute
        //costperminText.GetComponent<Text>().text = "Cost ($/min): " + System.Math.Round(costpermin, 3);
        m_econContent += "Cost ($/min): " + System.Math.Round(costpermin, 3) + "\n";

        // Apply text to economy view.
        TextMeshProUGUI tm;
        if(m_econView.TryGetComponent<TextMeshProUGUI>(out tm))
           tm.text = m_econContent;
    }

    // Toggles the visibility of the economy view window.
    private void ToggleEconWindow()
	{
        econViewWindow.SetActive(!econViewWindow.activeSelf);
	}

    // Reset economy values.
    private void ResetEcon()
	{
        Debug.Log("resetting economy window.");
		Laborcost = 0f;
		depreciation = 0;
		ColdFluidCost = 0;
		pumpingcost = 0;
		impellercost = 0;
		UVcost = 0;
		electricitycost = 0;
		NaOHcost = 0;
		totalcost = 0d;
		NewNaOHconsumed = 0;
		econtime = 0;
		resettime = runtime;

        Invoke("ExportEcon", 10);
    }

    // Create a new econ export file for this session.
    public void InitExportEcon()
    {
        System.DateTime dt = System.DateTime.Now;
        string timestamp = dt.Year.ToString() + '-' + dt.Month.ToString() + '-' + dt.Day.ToString() + '-' +
                           dt.Hour + dt.Minute + dt.Second;
        string filename = "reactor-econ-" + timestamp + ".csv";

        fs = new System.IO.StreamWriter(filename);

        // Write CSV header.
        string[] parameters = {
            "Time", // HH:MM:SS
            "pH",
            "Feed Flow Rate",
            "Cold Fluid Flow Rate",
            "Labor Cost",
            "Deprecation",
            "Cold Fluid Cost",
            "Pumping Cost",
            "Impeller Cost",
            "UV Cost", 
            "Electricity Cost",
            "NaOH Cost",
            "Total Cost",
            "New NaOH Consumed",
            "Tc-In",
            "Th-In",
            "Tc-Out",
            "Th-Out",
            "CA-Out",
        };

        string header = "";
        foreach(string val in parameters)
		{
            header += val + ',';
		}
        fs.WriteLine(header);
    }

    private void WriteEconLine()
    {
        string line = "";

        feed_script fscr = feed_script.GetComponent<feed_script>();

        line += timelapsed + ",";
        line += fscr.pHvalue + ",";
        line += fscr.F0 + ",";
        line += ColdFluidFlowRateVal.ToString() + ',';
        line += Laborcost.ToString() + ',';
        line += depreciation.ToString() + ',';
        line += ColdFluidCost.ToString() + ',';
        line += pumpingcost.ToString() + ',';
        line += impellercost.ToString() + ',';
        line += UVcost.ToString() + ',';
        line += electricitycost.ToString() + ',';
        line += NaOHcost.ToString() + ',';
        line += totalcost.ToString() + ',';
        line += NewNaOHconsumed.ToString() + ',';
        line += ColdFluidInputTempVal.ToString() + ',';
        line += HotFluidInputTempVal.ToString() + ',';
        line += ColdFluidInputTempVal.ToString() + ',';
        line += HotFluidOutTempVal.ToString() + ',';
        line += fscr.CA.ToString() + ',';

        fs.WriteLine(line);
    }

    private void OnDestroy()
    {
        if(fs != null)
            fs.Close();
    }
}
