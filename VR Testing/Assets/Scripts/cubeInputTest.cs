using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeInputTest : MonoBehaviour
{
    BoxCollider bc;

    // Start is called before the first frame update
    void Awake()
    {
        bc = GetComponent<BoxCollider>();
    }

    public void DisableCube(bool show) 
    {
        if (show)
            bc.enabled = true;
        else
            bc.enabled = false;
	}
}
