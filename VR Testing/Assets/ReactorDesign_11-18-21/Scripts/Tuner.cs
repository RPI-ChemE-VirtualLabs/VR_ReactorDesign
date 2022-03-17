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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (handInRange)//gameObject.GetComponent<Interactable>().isInteractable == true)
        {
            float newZ = hand.transform.localRotation.z;
            if(newZ > 0 && newZ < 180) {
                newZ = Mathf.Clamp(newZ, 0, 140);
                Percentage = (newZ + 180) / 280;
            }
            else {
                newZ = Mathf.Clamp(newZ, 360 - (140), 360);
                Percentage = (newZ - 180) / 280;
            }

            //Percentage = (newZ + 140) / 280;

            //Debug.Log(newZ);

            //Quaternion newRotation = Quaternion.Euler(-90, 0, newZ + 90);
            //transform.SetPositionAndRotation(transform.position, newRotation);

            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(-90, 0, 140 * Percentage + 90));

            Debug.Log(Percentage); 
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

	private void OnTriggerEnter(Collider other) {
        print("something touched the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            print("hand found, attached to dial");
            //assign gameObject to hand
            hand = other.gameObject; //to get controller instead of collider
            Debug.Log(hand.name);

            handInRange = true; //enable selection
        }
    }

	private void OnTriggerExit(Collider other) {
        print("something stopped touching the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            print("hand detatched from dial");
            handInRange = false; //disable selection
        }
    }
}
