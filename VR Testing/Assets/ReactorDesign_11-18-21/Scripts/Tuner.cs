using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuner : MonoBehaviour
{
    public float Percentage = 0f;

    public GameObject Character;
    public Material stairprops;
    public GameObject ResetCFTButton;
    
    [SerializeField]
    float sensitivity = .001f;
    [SerializeField]
    float sensitivityChangeDivide = 20;
    bool sensitivityChange = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Interactable>().isInteractable == true)
        {
            Cursor.lockState = CursorLockMode.None;
            float addPercent = -Input.GetAxis("Mouse X") * sensitivity;

            Percentage += addPercent;

            Percentage = Mathf.Clamp(Percentage, -1, 1);

            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(-90, 0, 140 * -Percentage - 90));

            Debug.Log("Mouse pos: " + Input.mousePosition.x);
            //Debug.Log(Percentage); 

            if (Input.GetMouseButtonUp(0))
            {
                gameObject.GetComponent<Interactable>().isInteractable = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            //press shift to lower percentage change sensitivity (toggle to make it consistent)
            if (Input.GetKeyDown(KeyCode.LeftShift)) 
            {
                if (!sensitivityChange)
                {
                    sensitivity /= sensitivityChangeDivide;
                    sensitivityChange = true;
                }
                else
                {
                    sensitivity *= sensitivityChangeDivide;
                    sensitivityChange = false;
                }
            }


            /*
            Turning();
            gameObject.GetComponent<Interactable>().isInteractable = false;

            ResetCFTButton.GetComponent<MeshRenderer>().material = stairprops;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit clickinfo = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out clickinfo))
                {
                    if (clickinfo.collider != null)
                    {
                        // ***********************************************************************
                        if (clickinfo.transform.gameObject.name == "ResetCFTButton")
                        {
                            Percentage = 0;
                            RotateObject.Rotate(0, 0, -1*Percentage);
                        }
                    }
                }
            }
            */
        }
    }


    /*void Turning()
    {
        if (Character.gameObject.GetComponent<CharacterRaycast>().isLeftMouseButton == true)
        {
            StartCoroutine("RotatingIncreasing");
        }

        if (Character.gameObject.GetComponent<CharacterRaycast>().isRightMouseButton == true)
        {
            StartCoroutine("RotatingDecreasing");
        }
    }*/

    /*
    private void Reset()
    {

        

    }

    IEnumerator RotatingIncreasing()
    {
        yield return new WaitForSeconds(0.3f);
        RotateObject.Rotate(0, 0, 0.36f, Space.Self);
        Percentage += 0.001f;

        if (Percentage >= 1f)
        {
            Percentage = 0;
        }
    }

    IEnumerator RotatingDecreasing()
    {
        yield return new WaitForSeconds(0.3f);
        RotateObject.Rotate(0, 0, -0.36f, Space.Self);
        Percentage -= 0.001f;
        if (Percentage <= 0f)
        {
            Percentage = 1;
        }

    }
    */
}
