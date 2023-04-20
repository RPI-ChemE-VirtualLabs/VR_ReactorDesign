using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdFluidFlowRate : MonoBehaviour
{
    public Tuner ColdFluidFlowRateTuner;
    public Text ColdFluidFlowRateText;
    public Text ColdFluidConsumedText;
    private float Value;
    public double ColdFluidFlowRateVal;

    public float runtime;
    public double CWconsumed;
   
    public float runtimeprev;

    public GameObject feed_script;


    public GameObject ResetCFFButton;
    public bool CFFbuttonpushed = false;
    public Material yellow;
    public Material stairprops;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        runtimeprev = runtime;
        runtime = feed_script.GetComponent<feed_script>().runtime;


        if (ColdFluidFlowRateTuner)
        {
            Value = ColdFluidFlowRateTuner.GetComponent<Tuner>().Percentage;
            ColdFluidFlowRateVal = 50 + Value * 1950; //Converting percentage to actual value, need to be replaced by the value of divison of range of data and 100
                                                      // kg/min  

            CWconsumed = CWconsumed + ColdFluidFlowRateVal * (runtime - runtimeprev)/60d; // kg


            ColdFluidFlowRateText.GetComponent<Text>().text = "Cold Fluid Flow Rate: " + System.Math.Round(ColdFluidFlowRateVal,0) + " kg/min";
            ColdFluidConsumedText.GetComponent<Text>().text = "Cold Fluid Consumed: " + System.Math.Round(CWconsumed, 0) + " kg";
        }


        ResetCFFButton.GetComponent<MeshRenderer>().material = stairprops;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "ResetCFFButton")

                    {

                        ResetCFFButton.GetComponent<MeshRenderer>().material = yellow;
                        ColdFluidFlowRateTuner.GetComponent<Tuner>().Percentage = 0;
                        ColdFluidFlowRateVal = 50f;
                        

                    }
                }

            }
        }

        ResetCFFButton.GetComponent<MeshRenderer>().material = stairprops;

        
    }
}
