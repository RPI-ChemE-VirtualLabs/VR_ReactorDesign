using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterRaycast : MonoBehaviour
{
    public bool isLeftMouseButton = false;
    public bool isRightMouseButton = false;
    
    private FirstPersonController FPSContoller;
    // Start is called before the first frame update
    void Start()
    {
        FPSContoller = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("LMB Triggered");
            isLeftMouseButton = true;
            CharacterRayCast();
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            isLeftMouseButton = false;
            FPSContoller.cameraEnabled = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("RMB Triggered");
            isRightMouseButton = true;
            CharacterRayCast();
            
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButton = false;
            FPSContoller.cameraEnabled = true;
        }
    }

    private void CharacterRayCast()
    {

        RaycastHit Hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Hit))
        {
            Debug.Log("Raycast Complete");
            if (Hit.collider != null) 
            {
                if (Hit.transform.gameObject.CompareTag("Interactable"))
                {
                    FPSContoller.cameraEnabled = false;
                    Hit.transform.gameObject.GetComponent<Interactable>().isInteractable = true;

                }
            }

        }
    }
}

