using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    public bool isInteractable = false;

    private void Update()
    {
        if (isInteractable)
        {
            Debug.Log("Interactable Triggered");
        }
    }
    
}
