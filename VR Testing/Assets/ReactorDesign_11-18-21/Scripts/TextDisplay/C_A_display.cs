using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_A_display : MonoBehaviour
{
    public GameObject impellerbutton;
    public GameObject UVbutton;
    public GameObject feedbutton;
    public GameObject feed_script;
    public Text CAtext;

    public bool impellerbuttonpushed_CA;
    public bool UVbuttonpushed_CA;
    public bool feedbuttonpushed_CA;


    public bool codecheck = false;
    public string mixerbuttonmaterial;
    public string UVbuttonmaterial;
    public string feedbuttonmaterial;

    public float CA;
    public float value;


    // Start is called before the first frame update
    void Start()
    {
        //feed_script = GameObject.FindWithTag("feed_script"); // find the GameObject with the script "feed_script" attached to it
    }

    // Update is called once per frame
    void Update()

    {

        mixerbuttonmaterial = impellerbutton.gameObject.GetComponent<Renderer>().material.name;
        UVbuttonmaterial = UVbutton.gameObject.GetComponent<Renderer>().material.name;
        feedbuttonmaterial = feedbutton.gameObject.GetComponent<Renderer>().material.name;

        if (mixerbuttonmaterial == "stop button (Instance)")
        {
            impellerbuttonpushed_CA = false;


        }

        else
        {
            impellerbuttonpushed_CA = true;


        }

        if (UVbuttonmaterial == "stop button (Instance)")
        {
            UVbuttonpushed_CA = false;


        }

        else
        {
            UVbuttonpushed_CA = true;


        }


        if (feedbuttonmaterial == "stop button (Instance)")
        {
            feedbuttonpushed_CA = false;


        }

        else
        {
            feedbuttonpushed_CA = true;

            
        }



       // if (impellerbuttonpushed_CA == true)
       // {


           


               if (feedbuttonpushed_CA == true)
                {

                    codecheck = true;



                    value = feed_script.GetComponent<feed_script>().CA; // retrieve "runtime" value from the other GameObject

                    CAtext.GetComponent<Text>().text = "CA (mol/m3): " + System.Math.Round(value,4); // send the value of runtime to the computer monitor

                }



           



       // }









    }
}
