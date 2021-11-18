using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mixerstatus : MonoBehaviour
{
    public GameObject impellerbutton;
    public GameObject UVbutton;
    public GameObject feedbutton;
    //public GameObject findF0;
    public GameObject feed_script; // new Sept14

    public Text statustext;
    public Text UVstatustext;
    public Text feedstatustext;

    public bool impellerbuttonpushed;
    public bool UVbuttonpushed;
    public bool feedbuttonpushed;

    //public float F0value;
    public bool codecheck = false;
    public string mixerbuttonmaterial;
    public string UVbuttonmaterial;
    public string feedbuttonmaterial;

    public float value2; // new Sept14

    // Start is called before the first frame update
    void Start()
    {
        //findF0 = GameObject.FindWithTag("feed_script"); // find the GameObject with the script "feed_script" attached to it
    }

    // Update is called once per frame
    void Update()

        // Fetch the value of F0 from feed_script

    {

    //    F0value = findF0.GetComponent<feed_script>().F0; // retrieve "F0" value from the other script


        mixerbuttonmaterial = impellerbutton.gameObject.GetComponent<Renderer>().material.name;
        UVbuttonmaterial = UVbutton.gameObject.GetComponent<Renderer>().material.name;
        feedbuttonmaterial = feedbutton.gameObject.GetComponent<Renderer>().material.name;

        if (mixerbuttonmaterial == "stop button (Instance)")
        {
            impellerbuttonpushed = false;
            
            statustext.GetComponent<Text>().text = "Mixer status: Off";
        }

        else
        {
            impellerbuttonpushed = true;
            
            statustext.GetComponent<Text>().text = "Mixer status: On";
        }

        if (UVbuttonmaterial == "stop button (Instance)")
        {
            UVbuttonpushed = false;
            
            UVstatustext.GetComponent<Text>().text = "UV source: Off";
        }

        else
        {
            UVbuttonpushed = true;
            
            UVstatustext.GetComponent<Text>().text = "UV source: On";
        }


        if (feedbuttonmaterial == "stop button (Instance)")
        {
            feedbuttonpushed = false;

            value2 = feed_script.GetComponent<feed_script>().F0set; // retrieve "F0set" value from the other GameObject  // new Sept14

            feedstatustext.GetComponent<Text>().text = "Feed flow (m3/min): Off"; 
        }

        if (feedbuttonmaterial == "start button (Instance)")
        {
            feedbuttonpushed = true;

            value2 = feed_script.GetComponent<feed_script>().F0set; // retrieve "F0set" value from the other GameObject  // new Sept14

            feedstatustext.GetComponent<Text>().text = "Feed flow (m3/min):" + System.Math.Round(value2, 4); // modified Sept14
        }





    }
}
