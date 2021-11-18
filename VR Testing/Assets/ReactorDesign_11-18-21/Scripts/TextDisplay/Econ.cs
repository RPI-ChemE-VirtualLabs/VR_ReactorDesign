using System.Collections;
using System.Collections.Generic;
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

    // public float ColdFluidInletTemp; // K
    public double costperkg = (0.056f/1000f); // cost of chilling water per kg

    public GameObject feed_script; // script
    public GameObject pHsettings; // script
    public GameObject ColdFluidFlowRate; // script
    public GameObject HotFluidFlowRate; // script
    public GameObject HotFluidOutTemp; // script
    public GameObject ColdFluidInputTemp;  //script
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
        runtime = feed_script.GetComponent<feed_script>().runtime;
        econtime = runtime - resettime;
        ResetEconButton.GetComponent<MeshRenderer>().material = stairprops;

        HotFluidOutTempVal = HotFluidOutTemp.GetComponent<HotFluidOutTemp>().HotFluidOutputVal;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "ResetEconButton")

                    { 
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
                        ResetEconButton.GetComponent<MeshRenderer>().material = yellow;
                        econtime = 0;
                        resettime = runtime;
                    }
                }

            }
        }

        

        pHvalue = pHsettings.GetComponent<pHsettings>().pHvalue;

        NaOHconc = pHsettings.GetComponent<pHsettings>().NaOHconc;

        NewNaOHconsumed = NewNaOHconsumed + NaOHconc * 1000f * 0.04f * (runtime - runtimeprev) * (F0/60); 

        NaOHconsumed = pHsettings.GetComponent<pHsettings>().NaOHconsumed; // flowrate in m^3/s from feed_script


        NaOHcost = NewNaOHconsumed*0.45f;  // based on current NaOH price of 45c per kg
        NaOHcostText.GetComponent<Text>().text = "NaOH Cost ($): " + System.Math.Round(NaOHcost, 3);

        impellerbuttonpushed = impeller_script.GetComponent<impeller_script>().impellerbuttonpushed;
        UVbuttonpushed = feed_script.GetComponent<feed_script>().UVbuttonpushed;
        feedbuttonpushed = feed_script.GetComponent<feed_script>().feedbuttonpushed;

        if (impellerbuttonpushed == true)
        {
            impfactor = 1;
        
        }
        else { impfactor = 0; }


        if (feedbuttonpushed == true)
        {
            feedfactor = 1;

        }
        else {feedfactor = 0; }


        if (UVbuttonpushed == true)
        {
            UVfactor = 1;

        }
        else {UVfactor = 0; }


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


            if (HotFluidOutTempVal < 330.1)
            {
            depreciation = depreciation + 0.00062f * (runtime - runtimeprev); // $/s

            }

            else
            {
                tempval = (float)HotFluidOutTempVal;
                depreciation = depreciation + 0.00062f * 5f*(runtime - runtimeprev) * Mathf.Exp((tempval - 330f) / 10f); // $/s
                

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

        DepreciationText.GetComponent<Text>().text = "Depreciation Cost ($): " + System.Math.Round(depreciation, 3);

        Laborcost = Laborcost + 0.0019f* (runtime - runtimeprev); //$/s
        LaborcostText.GetComponent<Text>().text = "Labor Cost ($): " + System.Math.Round(Laborcost, 3);

        ColdFluidFlowRateVal = ColdFluidFlowRate.GetComponent<ColdFluidFlowRate>().ColdFluidFlowRateVal; // in kg/min
        ColdFluidInputTempVal = ColdFluidInputTemp.GetComponent<ColdFluidInputTemp>().ColdFluidInputTempVal;
        costperkg = 0.000056 + ((20 - ColdFluidInputTempVal + 273) * 0.0175)/20000; 

        ColdFluidCost = ColdFluidCost + feedfactor*(lastframetime * costperkg * ColdFluidFlowRateVal/60d);
        ColdFluidCostText.GetComponent<Text>().text = "Chilling Water Cost ($): " + System.Math.Round(ColdFluidCost, 3);

        

        CWConsumed = ColdFluidFlowRate.GetComponent<ColdFluidFlowRate>().CWconsumed;
        HWConsumed = HotFluidFlowRate.GetComponent<HotFluidFlowRate>().HWconsumed;

        F0 = feed_script.GetComponent<feed_script>().F0;
        pumpingcost = pumpingcost + 0.1785*(F0)*(runtime-runtimeprev)/(3600d) + 0.1785*(ColdFluidFlowRateVal/1000d)*(runtime - runtimeprev)/(3600d); // $/s based on $0.1785 /hr with F0 = 1 m^3/min
        impellercost = impellercost + 0.056d*3.000d * (runtime - runtimeprev) / (3600d); // $/s assumed 3.000 kW power consumption
        UVcost = UVcost + 0.056d*1.000d * (runtime - runtimeprev) / (3600d); // $/s assumed 1.000 kW power consumption
        electricitycost = pumpingcost + impellercost + UVcost;  // $/s

        totalcost = electricitycost + depreciation + Laborcost + NaOHcost + ColdFluidCost;

        electricitycostText.GetComponent<Text>().text = "Electricity Cost ($): " + System.Math.Round(electricitycost, 3);
        totalcostText.GetComponent<Text>().text = "Total Cost ($): " + System.Math.Round(totalcost, 3);
        econtimeText.GetComponent<Text>().text = "Econ. time (s): " + System.Math.Round(econtime, 3);

        costpermin = 60*totalcost / econtime; // $ per minute
        costperminText.GetComponent<Text>().text = "Cost ($/min): " + System.Math.Round(costpermin, 3);




    }
}
