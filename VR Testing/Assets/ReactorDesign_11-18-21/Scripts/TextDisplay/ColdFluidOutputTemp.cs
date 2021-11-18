using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdFluidOutputTemp : MonoBehaviour
{
    public HotFluidInputTemp HotFluidInputTempVal;
    public Text ColdFluidOutputText;
    public ColdFluidInputTemp ColdFluidInputTempVal;
    public HotFluidFlowRate HotFluidFlowRateVal;
    public ColdFluidFlowRate ColdFluidFlowRateVal;

    public GameObject HotFluidOutTemp;

    private double HotFluidInputTempValue = 0f;
    private double ColdFluidInputTempValue = 0f;
    private double HotFluidFlowRateValue = 0f;
    private double ColdFluidFlowRateValue = 0f;

    public double ColdFluidOutputVal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((HotFluidInputTempVal != null) && (ColdFluidInputTempVal != null))
        {
            HotFluidInputTempValue = HotFluidInputTempVal.HotFluidInputTempVal;
            ColdFluidInputTempValue = ColdFluidInputTempVal.ColdFluidInputTempVal;

            HotFluidFlowRateValue = HotFluidFlowRateVal.HotFluidFlowRateVal;
            ColdFluidFlowRateValue = ColdFluidFlowRateVal.ColdFluidFlowRateVal;

            ColdFluidOutputVal = HotFluidOutTemp.GetComponent<HotFluidOutTemp>().Tcout;  //  in m3/min
                                                                                            // HotFluidInputTempValue * HotFluidFlowRateValue + ColdFluidInputTempValue * ColdFluidFlowRateValue;//placeholder function
            ColdFluidOutputText.GetComponent<Text>().text = "Cold Fluid Outlet Temp.: " + System.Math.Round(ColdFluidOutputVal,2) + " K";
        }
    }
}
