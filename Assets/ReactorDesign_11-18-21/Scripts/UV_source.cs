using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class UV_source : MonoBehaviour
{

    public GameObject UVbutton;
    public GameObject UVwindow;
    public bool UVbuttonpushed = false;

    public Material shinypipe;
    public Material water2;
    public Material off;
    public Material on;

    public string UVbuttoncolor;
    public float rateconstant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickinfo = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out clickinfo))
            {
                if (clickinfo.collider != null)
                {
                    // ***********************************************************************
                    if (clickinfo.transform.gameObject.name == "UVbutton")  // buttontemplate now turns on the control system

                    {
                        if (UVbuttonpushed)
                        {
                            UVbuttonpushed = false;
                            UVbutton.GetComponent<MeshRenderer>().material = off;
                            UVwindow.GetComponent<MeshRenderer>().material = shinypipe;

                        }
                        else
                        {
                            UVbuttonpushed = true;
                            UVbutton.GetComponent<MeshRenderer>().material = on;
                            UVwindow.GetComponent<MeshRenderer>().material = water2;
                        }
                    }



                }


            }
        }

        UVbuttoncolor = UVbutton.gameObject.GetComponent<Renderer>().material.name; // gets the name of the material from the blue atom contacted

        if (UVbuttonpushed == true)
        {
            rateconstant = 1f;

        }

        else
        {
            rateconstant = 1e-5f;
        }



    }
}


