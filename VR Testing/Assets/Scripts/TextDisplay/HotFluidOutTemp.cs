using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotFluidOutTemp : MonoBehaviour
{
    public HotFluidInputTemp HotFluidInputTempVal;
    public Text HotFluidOutputText;
    public ColdFluidInputTemp ColdFluidInputTempVal;
    public HotFluidFlowRate HotFluidFlowRateVal;
    public ColdFluidFlowRate ColdFluidFlowRateVal;

    private double HotFluidInputTempValue = 0f;
    private double ColdFluidInputTempValue = 0f;
    private double HotFluidFlowRateValue = 0f;
    private double ColdFluidFlowRateValue = 0f;

    public float HotFluidOutputVal;
   
    public float feedflow; // kg/s
    public float lastval;


    public GameObject feed_script;

    public float[] Tcout_guess = new float[201];
    public float[] Thout_guess = new float[201];
    public float[] Qdot_guess = new float[201];
    public float[] Qdot_calc = new float[201];
    public float[] LMTD_guess = new float[201];
    public float[] Qdiff = new float[201];
    public float[] DeltaTA = new float[201];
    public float[] DeltaTB = new float[201];
    public float DTA_ans;
    public float DTB_ans;
    public float Qdiff_min;
    public float Thout;
    public float Tcout;
    public float Qdot;
    public float UAvalue = 1200; // as in Q = UA*LMTD;

    public float check_coldflow;
    public float check_coldtempin;
    public float check_hottempin;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 1; i < 201; i++)
         {
            Tcout_guess[i] = 273 + 0.5f*i;  // List of values that Tcout can have
 
         }

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

            feedflow = feed_script.GetComponent<feed_script>().F0set;  //  in m3/min

            
          
            
            // Solve heat exchanger outlet temperatures

            for (int i = 1; i < 201; i++)
            {

                check_coldflow = (float)ColdFluidFlowRateValue;  // kg/min
                check_coldtempin = (float)ColdFluidInputTempValue;  // K
                check_hottempin = (float)HotFluidInputTempValue;  // K


                Qdot_guess[i] = (4.184f) * (float)ColdFluidFlowRateValue * (Tcout_guess[i] - (float)ColdFluidInputTempValue);  // kJ/min
                Thout_guess[i] = (-1f*Qdot_guess[i] / (feedflow * 1000f * 4.184f)) + (float)(HotFluidInputTempValue);
                DeltaTA[i] = ((float)HotFluidInputTempValue - Tcout_guess[i]);
                DeltaTB[i] = (Thout_guess[i] - (float)ColdFluidInputTempValue);
                LMTD_guess[i] = ( DeltaTA[i] - DeltaTB[i]) / ((float)System.Math.Log(DeltaTA[i]) - (float)System.Math.Log(DeltaTB[i]));
                Qdot_calc[i] = UAvalue * LMTD_guess[i];
                Qdiff[i] = Mathf.Abs(Qdot_calc[i] - Qdot_guess[i]);
                

            }

            Qdiff[0] = 1e13f;  // Always equal to zero, so element 0 will always be the minimum otherwise
            Qdiff_min = Mathf.Min(Qdiff);


            for (int i = 1; i < 201; i++)
            {
                if (Qdiff[i] == Qdiff_min)
                {

                    Thout = Thout_guess[i];
                    Tcout = Tcout_guess[i];
                    Qdot = Qdot_guess[i];
                    DTA_ans = DeltaTA[i];
                    DTB_ans = DeltaTB[i];
                }

                
            }

            HotFluidOutputVal = Thout; // HotFluidInputTempValue * HotFluidFlowRateValue + ColdFluidInputTempValue * ColdFluidFlowRateValue;//placeholder function
            
            HotFluidOutputText.GetComponent<Text>().text = "Hot Fluid Outlet Temp.: " + System.Math.Round(HotFluidOutputVal,1) + " K";
        
        }
    }
}
