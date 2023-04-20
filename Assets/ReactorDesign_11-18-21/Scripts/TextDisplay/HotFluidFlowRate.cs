using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotFluidFlowRate : MonoBehaviour
{

    // public Tuner HotFluidFlowRateTuner;
    public Text HotFluidFlowRateText;
    public float HFValue;
    public double HotFluidFlowRateVal;
    public GameObject feed_script;
    public float F0set;
    public bool feedbuttonpushed = false;

    public Text HotFluidConsumedText;
    public float runtime;
    public double HWconsumed;

    public float runtimeprev;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        runtimeprev = runtime;
        runtime = feed_script.GetComponent<feed_script>().runtime;

        HFValue = 1000f * feed_script.GetComponent<feed_script>().F0set; // HF fow rate in kg/s instead of m3/s
        HotFluidFlowRateText.GetComponent<Text>().text = "Hot Fluid Flow Rate: " + System.Math.Round(HFValue, 2) + " kg/min";


        feedbuttonpushed = feed_script.GetComponent<feed_script>().feedbuttonpushed;

        if (feedbuttonpushed == true)
        {
            HWconsumed = HWconsumed + HFValue * (runtime - runtimeprev) / 60d; // kg

        }

        
        HotFluidConsumedText.GetComponent<Text>().text = "Feed Consumed: " + System.Math.Round(HWconsumed, 0) + " kg";


    }
}
