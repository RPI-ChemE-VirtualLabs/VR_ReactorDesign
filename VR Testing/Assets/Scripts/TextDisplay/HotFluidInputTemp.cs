using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotFluidInputTemp : MonoBehaviour
{
    public Tuner HotFluidTuner;
    public Text HotFluidInputTempText;
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
		HotFluidInputTempVal = 353 - Value * 100; //Converting percentage of tuner to actual temperature
		HotFluidInputTempText.GetComponent<Text>().text = "Hot Fluid Inlet Temp.: " + System.Math.Round(HotFluidInputTempVal,2) + " K"; //Output to display or HUD
    }
}
