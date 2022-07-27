using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    GameObject hand;
    bool handInRange = false;
    bool VRTriggerDown = false;

    // Update is called once per frame
    void Update()
    {
        if (handInRange)// && VRTriggerDown)//gameObject.GetComponent<Interactable>().isInteractable == true)
        {
            //flip hand's transform along x axis to get desired rotation
            Transform handT = hand.transform;
            handT.RotateAround(transform.position, Vector3.left, 180.0f);
            float newZ = handT.transform.rotation.eulerAngles.z;

            //throw out input beyond or below a certain threshold. prevents annoying flipping at extremes
            float threshold = .2f;
            if(!(360 - newZ < 360 * threshold || 360 - newZ > 360 * (1.0f - threshold)))
                Percentage = (360 - newZ) / 360;

            Debug.Log("hand rotation at degree " + newZ + ", percentage " + Percentage);

            //clamp percentage
            //Percentage = Mathf.Clamp(Percentage, .2f, .8f);

            //Quaternion newRotation = Quaternion.Euler(-90, 0, newZ + 90);
            //transform.SetPositionAndRotation(transform.position, newRotation);

            //transform.SetPositionAndRotation(transform.position, Quaternion.Euler(-90, 0, 140 * Percentage + 90));

            //Debug.Log(Percentage); 
            /*
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
            */
        }
    }

    public void VRTriggerChange(bool state) {
        Debug.Log("VRTriggerDown changed to " + VRTriggerDown);
        VRTriggerDown = state;
	}

    private void OnTriggerEnter(Collider other) {
        //print("something touched the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            //print("hand found, attached to dial");
            //assign gameObject to hand
            hand = other.gameObject; //to get controller instead of collider
            Debug.Log(hand.name);

            handInRange = true; //enable selection
        }
    }

	private void OnTriggerExit(Collider other) {
        //print("something stopped touching the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            //print("hand detatched from dial");
            handInRange = false; //disable selection
        }
    }
}
