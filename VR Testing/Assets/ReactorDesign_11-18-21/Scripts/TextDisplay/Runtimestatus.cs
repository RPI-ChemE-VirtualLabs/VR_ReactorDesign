using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runtimestatus : MonoBehaviour
{
    public GameObject impellerbutton;
    public GameObject UVbutton;
    public GameObject feedbutton;
    public GameObject feed_script;
    public Text runtimetext;

    public bool impellerbuttonpushed = false;
    public bool UVbuttonpushed = false;
    public bool feedbuttonpushed = false;
 

    public bool codecheck = false;
    public string mixerbuttonmaterial;
    public string UVbuttonmaterial;
    public string feedbuttonmaterial;

    public float runtime;
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
            impellerbuttonpushed = false;

            
        }

        else
        {
            impellerbuttonpushed = true;

            
        }

        if (UVbuttonmaterial == "stop button (Instance)")
        {
            UVbuttonpushed = false;

            
        }

        else
        {
            UVbuttonpushed = true;

            
        }


        if (feedbuttonmaterial == "stop button (Instance)")
        {
            feedbuttonpushed = false;

            
        }

        else
        {
            feedbuttonpushed = true;

            // feedstatustext.GetComponent<Text>().text = "Feed flow: On";
        }




        if (feedbuttonpushed == true)
                {

                    codecheck = true;

                    

                    value = feed_script.GetComponent<feed_script>().runtime; // retrieve "runtime" value from the other GameObject

                    runtimetext.GetComponent<Text>().text = "Runtime (s): " + System.Math.Round(value,1); // send the value of runtime to the computer monitor

                }
        else
        {
            runtimetext.GetComponent<Text>().text = "Runtime (s):"; // send the value of runtime to the computer monitor
        }

        

            

        

       









    }
}
