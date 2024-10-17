using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV_source : MonoBehaviour
{
    public GameObject UVbutton;
    public GameObject UVwindow;

    /*
    public Material shinypipe;
    public Material water2;
    public Material off;
    public Material on;
    */
    public bool isEnabled
    {
        get;
        protected set;
    } = false;

    // TODO: Swap material instead of color.
    [SerializeField] Color buttonGlow;
    public float rateconstant = 1e-5f;

    public void EnableUV()
	{
        rateconstant = 1f;
        UVwindow.GetComponent<Renderer>().material.SetColor("_Color", buttonGlow);
        isEnabled = true;
	}

    public void DisableUV()
	{
        rateconstant = 1e-5f;
        UVwindow.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
        isEnabled = false;
	}
}


