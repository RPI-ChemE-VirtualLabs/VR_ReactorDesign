using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class impeller_script : MonoBehaviour
{

    public GameObject impellerbutton;
    public Material off;
    public Material on;
    public string buttoncolor;
    public bool impellerbuttonpushed = false;
    public AudioSource impellershaft;
    public AudioClip impellersound;

    private float soundvol = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        impellershaft = GetComponent<AudioSource>();
        // impellersound.Pause();

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
                    if (clickinfo.transform.gameObject.name == "impellerbutton")  // buttontemplate now turns on the control system

                    {
                        if (impellerbuttonpushed)
                        {
                            impellerbuttonpushed = false;
                            impellerbutton.GetComponent<MeshRenderer>().material = off;

                        }
                        else
                        {
                            impellerbuttonpushed = true;
                            impellerbutton.GetComponent<MeshRenderer>().material = on;
                        }
                    }



                }


            }
        }

        buttoncolor = impellerbutton.gameObject.GetComponent<Renderer>().material.name; // gets the name of the material from the blue atom contacted

        if (impellerbuttonpushed == true)
        {
            soundvol = 0.04f;
            transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);

            //impellershaft.PlayOneShot(impellersound, 1f);
            AudioSource.PlayClipAtPoint(impellersound, new Vector3(32.1f, 2.94f, 28.1f), soundvol);
        }

        else
        {
            transform.Rotate(new Vector3(0, 0, 0) * Time.deltaTime);
            soundvol = 0.0f;
            impellershaft.Stop();

            //impellersound.Pause();
        }



    }
}
