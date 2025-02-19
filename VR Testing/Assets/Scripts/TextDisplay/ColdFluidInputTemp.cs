using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdFluidInputTemp : MonoBehaviour
{
    //public Tuner ColdFluidTuner;
    //public Text ColdFluidInputTempText;
    [SerializeField] private VRKeypad kp;
    private float Value;
    public double ColdFluidInputTempVal;
    
    public GameObject ResetCFTButton;
    public bool CFTbuttonpushed =false;
    public Material yellow;
    public Material stairprops;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ColdFluidInputTempVal = kp.currentValue; //Converting percentage of tuner to actual temperature
        /*
        ResetCFTButton.GetComponent<MeshRenderer>().material = stairprops;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "ResetCFTButton")

                    {

                        ResetCFTButton.GetComponent<MeshRenderer>().material = yellow;
                        ColdFluidTuner.GetComponent<Tuner>().Percentage = 0;
                        ColdFluidInputTempVal = 273f; //Converting percentage of tuner to actual temperature
                        

                    }
                }

            }
        }

        ResetCFTButton.GetComponent<MeshRenderer>().material = stairprops;
        */
        


    }

    





}
