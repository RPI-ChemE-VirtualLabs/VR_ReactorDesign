using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pHsettings : MonoBehaviour
{

    public GameObject pH7button;
    public GameObject pH10button;
    public GameObject pH12button;
    public GameObject feed_script; // script
    public GameObject feedbutton;
    public GameObject HotFluidFlowRate; // script

    public bool pH7buttonpushed = false;
    public bool pH10buttonpushed = false;
    public bool pH12buttonpushed = false;
    public bool feedbuttonpushed_pH;
    public bool codecheck;

    public Material off;
    public Material on;

    public string pH7buttoncolor;
    public string pH10buttoncolor;
    public string pH12buttoncolor;
    public string feedbuttonmaterial;

    public Text pHtext;
    public Text NaOHtext;

    public int pHvalue = 7;
    public int counter = 0;

    public float runtime;
    public float NaOHconsumed;
    public float NaOHconc =0.0f;  // mol/L
    public float runtimeprev;
    public float flowrate = 0.1f; // flow rate, m^3/s - needs to be borrowed from HEX script
    public float F0set;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        feedbuttonmaterial = feedbutton.gameObject.GetComponent<Renderer>().material.name;
        runtimeprev = runtime;
        runtime = feed_script.GetComponent<feed_script>().runtime;

        // flowrate = HotFluidFlowRate.GetComponent<HotFluidFlowRate>().HFValue;
        flowrate = feed_script.GetComponent<feed_script>().F0set; // flowrate in m^3/min from feed_script
        NaOHconsumed = NaOHconsumed + NaOHconc * 1000f * 0.04f * (runtime - runtimeprev) * (flowrate/60); 

        if (feedbuttonmaterial == "stop button (Instance)")
        {
            feedbuttonpushed_pH = false;

            pH7buttonpushed = false;
            pH10buttonpushed = false;
            pH10button.GetComponent<MeshRenderer>().material = off;
            pH12buttonpushed = false;
            pH12button.GetComponent<MeshRenderer>().material = off;
            pH7button.GetComponent<MeshRenderer>().material = off;
            pHvalue = 7;
            NaOHconc = 0.0f; // mol/L



        }

        else
        {
            feedbuttonpushed_pH = true;

            if (pHvalue == 7)
            {
                pH7buttonpushed = true;
                pH10buttonpushed = false;
                pH10button.GetComponent<MeshRenderer>().material = off;
                pH12buttonpushed = false;
                pH12button.GetComponent<MeshRenderer>().material = off;
                pH7button.GetComponent<MeshRenderer>().material = on;
                pHvalue = 7;

            }

            if (pHvalue == 10)
            {
                pH10buttonpushed = true;
                pH10button.GetComponent<MeshRenderer>().material = on;
                pHvalue = 10;
                NaOHconc = 0.0001f; // mol/L
                pH7buttonpushed = false;
                pH7button.GetComponent<MeshRenderer>().material = off;
                pH12buttonpushed = false;
                pH12button.GetComponent<MeshRenderer>().material = off;

            }

            if (pHvalue == 12)
            {
                pH12buttonpushed = true;
                pH12button.GetComponent<MeshRenderer>().material = on;
                pHvalue = 12;
                NaOHconc = 0.01f; // mol/L
                pH10buttonpushed = false;
                pH10button.GetComponent<MeshRenderer>().material = off;
                pH7buttonpushed = false;
                pH7button.GetComponent<MeshRenderer>().material = off;

            }



        }

        //*************************************


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "pH7button")

                    {
                        if (pH7buttonpushed)
                        {
                            pH7buttonpushed = false;
                            pH7button.GetComponent<MeshRenderer>().material = off;


                        }
                        else
                        {
                            pH7buttonpushed = true;
                            pH10buttonpushed = false;
                            pH10button.GetComponent<MeshRenderer>().material = off;
                            pH12buttonpushed = false;
                            pH12button.GetComponent<MeshRenderer>().material = off;
                            pH7button.GetComponent<MeshRenderer>().material = on;
                            pHvalue = 7;
                            NaOHconc = 0.0f;
                        }
                    }
                }

            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "pH10button")

                    {
                        if (pH10buttonpushed)
                        {
                            pH10buttonpushed = false;
                            pH10button.GetComponent<MeshRenderer>().material = off;


                        }
                        else
                        {
                            pH10buttonpushed = true;
                            pH10button.GetComponent<MeshRenderer>().material = on;
                            pHvalue = 10;
                            NaOHconc = 0.0001f;
                            pH7buttonpushed = false;
                            pH7button.GetComponent<MeshRenderer>().material = off;
                            pH12buttonpushed = false;
                            pH12button.GetComponent<MeshRenderer>().material = off;
                        }
                    }
                }

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "pH12button")

                    {
                        if (pH12buttonpushed)
                        {
                            pH12buttonpushed = false;
                            pH12button.GetComponent<MeshRenderer>().material = off;


                        }
                        else
                        {
                            pH12buttonpushed = true;
                            pH12button.GetComponent<MeshRenderer>().material = on;
                            pHvalue = 12;
                            NaOHconc = 0.01f;
                            pH10buttonpushed = false;
                            pH10button.GetComponent<MeshRenderer>().material = off;
                            pH7buttonpushed = false;
                            pH7button.GetComponent<MeshRenderer>().material = off;
                        }
                    }
                }

            }
        }




        if (feedbuttonpushed_pH == true)
        {

            codecheck = true;

            pHtext.GetComponent<Text>().text = "pH = " + pHvalue + " (feed activated)"; // send the value of pH to the computer monitor
            NaOHtext.GetComponent<Text>().text = "NaOH used (kg) " + System.Math.Round(NaOHconsumed, 6); // send the value of NaOH consumed to the computer monitor
        }

        else
        {
            pHtext.GetComponent<Text>().text = "pH: 7 (feed not activated)"; // send the value of runtime to the computer monitor
            NaOHtext.GetComponent<Text>().text = "NaOH used (kg)"; // send the value of runtime to the computer monitor
        }


    }
}
