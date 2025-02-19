using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotFluidInputTemp : MonoBehaviour
{
    //public Tuner HotFluidTuner;
    //public Text HotFluidInputTempText;
    [SerializeField] private VRKeypad kp;
    [SerializeField] private float Value;
    public double HotFluidInputTempVal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Value = HotFluidTuner.GetComponent<Tuner>().Percentage;
		HotFluidInputTempVal = kp.currentValue; //Converting percentage of tuner to actual temperature
		//HotFluidInputTempText.GetComponent<Text>().text = "Hot Fluid Inlet Temp.: " + System.Math.Round(HotFluidInputTempVal,2) + " K"; //Output to display or HUD
    }
}
