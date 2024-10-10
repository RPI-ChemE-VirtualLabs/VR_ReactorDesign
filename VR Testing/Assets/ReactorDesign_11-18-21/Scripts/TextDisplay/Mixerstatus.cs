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
    [SerializeField] GameObject feed_script;
    private feed_script fs;

    public Text statustext;
    public Text UVstatustext;
    public Text feedstatustext;

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
        if(!feed_script.TryGetComponent<feed_script>(out fs))
            Debug.LogError("Mixer Status couldn't find the feed script.");
    }

    // Update is called once per frame
    void Update()
    {
        /* Ternary statements:
         *   fs.Impellerbuttonpushed ? "On" : "Off"
         * is essentially:
         * if(fs.Impellerbuttonpushed)
         *  return "On";
         * else
         *  return "Off";
         */
        statustext.text = "Mixer status: " + (fs.impellerOn ? "On" : "Off");

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
