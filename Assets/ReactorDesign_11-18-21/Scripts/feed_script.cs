﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feed_script : MonoBehaviour
{

    public GameObject feedbutton;
    public GameObject UV_source;
    public GameObject feedupbutton;
    public GameObject feeddownbutton;
    public GameObject pHsettings;
    public GameObject impellerbutton;
    public GameObject HotFluidOutTemp;
    // public GameObject Impeller_script;

    public bool feedbuttonpushed = false;
    public bool feedon;
    public bool running_CA = false;
    public bool feedupbuttonpushed = false;
    public bool feeddownbuttonpushed = false;
    public bool UVbuttonpushed = false;
    public bool Impellerbuttonpushed = false;
    public Material off;
    public Material on;
    public string mixing;

    public string feedbuttoncolor;
    public float rateconstant;
    public float GetRuntime() { return runtime; }
    public float k;
    public float multiplier =0.01f;

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
    public float CA = 0f;
    public float VR = 10f; // reactor volume, m^3
    public float nA = 0f;
    public float F0 = 0; // m^3 / min
    public float F0set = 0.5f; // m^3 / min
    public float dnAdt = 0;
    public float CAin = 0f;
    public float oldruntime = 0f;
    public float pHvalue = 7f;
    public float Ea_R = 1.35e4f;
    public float rxntemp;

    public int counter = 0;
    public int counter2 = 0;
    public int rcounter = 0;  //  # times Record button pushed. 0 = none; 1 = recording; 2 = stop recording, reset counter to 0
    public int framecounter = 0;

    public float[] runtimearray = new float[1000];
    public float[] Conc_array = new float[1000];

    // Start is called before the first frame update
    void Start()
    {
        begintime += Time.deltaTime;
        Application.targetFrameRate = 60;
        k = 0.0f;
        nA = 0f;
        F0 = 0f;
        F0set = 0.5f;
        CA = nA / VR;

        //UV_source = GameObject.FindWithTag("UV_source (script)"); // find the GameObject with the script "UV-source" attached to it
    }

    // Update is called once per frame
    void Update()
    {
        pHvalue = pHsettings.GetComponent<pHsettings>().pHvalue;
        UVbuttonpushed = UV_source.GetComponent<UV_source>().UVbuttonpushed;
        mixing = impellerbutton.gameObject.GetComponent<Renderer>().material.name; // gets the name of the material from the blue atom contacted
        rxntemp = HotFluidOutTemp.gameObject.GetComponent<HotFluidOutTemp>().Thout;

        if (mixing == "stop button (Instance)")
        {
            Impellerbuttonpushed = false;
            multiplier = 0.01f;
        }

        if (mixing == "start button (Instance)")
        {
            Impellerbuttonpushed = true;
            multiplier = 1.0f;
        }

        if (UVbuttonpushed == true)
        {
            if (pHvalue == 7)
            {
                // k = (0.02612f)*multiplier*Mathf.Exp(-1f*Ea_R/rxntemp); // min^-1
                k = (2.6115e18f)*multiplier*Mathf.Exp(-1f*Ea_R/rxntemp); // min^-1
            }
            if (pHvalue == 10)
            {
                //k = (0.05118f)*multiplier;
                k = (5.117e18f) * multiplier * Mathf.Exp(-1f * Ea_R / rxntemp); // min^-1
            }
            if (pHvalue == 12)
            {
                //k = (0.05588f*multiplier);
                k = (5.5869e18f) * multiplier * Mathf.Exp(-1f * Ea_R / rxntemp); // min^-1
            }
        }
        else
        {
            k = 0.0f;
        }

        if (feedbuttonpushed == true)
        {
            F0 = F0set;
        }

        else
        {
            F0 = 0f;
        }   

        // ***** Timekeeping operations
        frameselapsed = frameselapsed + 1.0f;
        timelapsed_prev = timelapsed;
        timelapsed += Time.deltaTime;
        lastframetime = timelapsed - timelapsed_prev;
        framerate = frameselapsed / timelapsed;
        frameselapsed = frameselapsed + 1.0f;
        timelapsed += Time.deltaTime;
        framerate = frameselapsed / timelapsed;

        // Determine if UV source is turned on
        UVbuttonpushed = UV_source.GetComponent<UV_source>().UVbuttonpushed; // retrieve "UVbuttonpushed" value from the other GameObject

        // Determine if feed flow rate setpoint is being changed
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo2 = new RaycastHit();
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray2, out clickinfo2))
            {
                if (clickinfo2.collider != null)
                {
                        // ***********************************************************************
                        if (clickinfo2.transform.gameObject.name == "feedupbutton")

                        {
                            if (feedupbuttonpushed)
                            {
                                feedupbuttonpushed = false;
                                //feedupbutton.GetComponent<MeshRenderer>().material = off;

                                if (F0set < 0.95f)
                                {
                                    F0set = F0set + 0.1f;
                                }


                            }
                            else
                            {
                                feedupbuttonpushed = true;
                                //feedupbutton.GetComponent<MeshRenderer>().material = on;

                                if (F0set < 0.95f)
                                {
                                    F0set = F0set + 0.1f;
                                }


                            }
                        }

                        //**********************************************************
                        if (clickinfo2.transform.gameObject.name == "feeddownbutton")

                        {
                            if (feeddownbuttonpushed)
                            {
                                feeddownbuttonpushed = false;
                                //feeddownbutton.GetComponent<MeshRenderer>().material = off;

                                if (F0set > 0.15f)
                                {
                                    F0set = F0set - 0.1f;
                                }


                            }
                            else
                            {
                                feeddownbuttonpushed = true;
                                //feeddownbutton.GetComponent<MeshRenderer>().material = on;

                                if (F0set > 0.15f)
                                {
                                    F0set = F0set - 0.1f;
                                }


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
                        if (clickinfo.transform.gameObject.name == "feedbutton")

                        {
                            if (feedbuttonpushed)
                            {
                                feedbuttonpushed = false;
                                feedbutton.GetComponent<MeshRenderer>().material = off;


                            }
                            else
                            {
                                feedbuttonpushed = true;
                                feedbutton.GetComponent<MeshRenderer>().material = on;

                            }
                        }
                    }

                }
            }

        feedbuttoncolor = feedbutton.gameObject.GetComponent<Renderer>().material.name; // gets the name of the material from the feedbutton

        feedbuttonpushed = feedon;

        if (feedbuttonpushed == true)
            counter++;

        if (counter > 0)
        {
            runtime += Time.deltaTime;
        }

            // Determine CA and perform mass balances
            if (feedbuttonpushed == true)
            {
                CAin = 2f;

                if (UVbuttonpushed == true)
                {

                    running_CA = true;

                    CA = nA / VR;

                    dnAdt = F0 * (CAin - CA) - k * CA * VR;

                    nA = nA + dnAdt * (runtime - oldruntime);

                    CA = nA / VR;

                }

                else
                {

                    running_CA = true;

                    CA = nA / VR;

                    dnAdt = F0 * (CAin - CA);

                    nA = nA + dnAdt * (runtime - oldruntime);

                    CA = nA / VR;

                }


            }



            oldruntime = runtime;


        }
    }

/*
internal class Impeller_script
{
    internal bool impellerbuttonpushed;
}
*/