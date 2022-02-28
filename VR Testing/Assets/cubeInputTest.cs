using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeInputTest : MonoBehaviour
{
    MeshRenderer ms;

    // Start is called before the first frame update
    void Awake()
    {
        ms = GetComponent<MeshRenderer>();
    }

    public void DisableCube(bool show) 
    {
        if (show)
            ms.enabled = true;
        else
            ms.enabled = false;
	}
}
