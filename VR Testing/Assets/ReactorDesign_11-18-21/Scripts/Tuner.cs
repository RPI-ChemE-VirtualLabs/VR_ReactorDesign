using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuner : MonoBehaviour
{
    public float Percentage = 0f;

    public GameObject Character;
    public Material stairprops;
    //public GameObject ResetCFTButton; the computer is yelling at me too much :(
    //i'm guessing this has to be manually assigned but. in the mean time i would like to Use The Debug Logs

    [SerializeField]
    float sensitivity = .001f;
    [SerializeField]
    float sensitivityChangeDivide = 20;
    bool sensitivityChange = false;

    GameObject hand = null;
    bool whichHand = false; // false = left, true = right
    bool handInRange = false;
    bool lTriggerDown = false;
    bool rTriggerDown = false;

    void Awake()
	{
        VR_CharacterController.triggerLeft += OnLeftTrigger;
        VR_CharacterController.triggerRight += OnRightTrigger;
	}

	// Update is called once per frame
	void Update() {
        //Debug.Log("i should be working: " + (hand != null && (lTriggerDown || rTriggerDown)));
        //Debug.Log("hand != null: " + (hand != null));
        //Debug.Log("lTriggerDown || rTriggerDown: " + (lTriggerDown || rTriggerDown));
        if (hand != null && ((lTriggerDown && !whichHand) || (rTriggerDown && whichHand)))// && VRTriggerDown)
        {
            //flip hand's transform along x axis to get desired rotation
            Transform handT = hand.transform;
            handT.RotateAround(transform.position, Vector3.left, 180.0f);
            float newZ = handT.transform.rotation.eulerAngles.z;
            Debug.Log(newZ);

            //throw out input beyond or below a certain threshold. prevents annoying flipping at extremes
            float threshold = .2f;
            if (!(360 - newZ < 360 * threshold || 360 - newZ > 360 * (1.0f - threshold))) {
                Percentage = (360 - newZ) / 360;
                //calculate rotation delta and rotate about y
                float diff = (360 - newZ) - (transform.localRotation.eulerAngles.z + 180);
                transform.Rotate(0, 0, diff); //did you know! i hate eulers with a burning passion
            }
        }
    }

    public void OnLeftTrigger(float pressure) {
        if (!whichHand)
        {
            lTriggerDown = pressure < .1f ? false : true;
            //Debug.Log("lTriggerDown changed to " + lTriggerDown);
        }
    }

    public void OnRightTrigger(float pressure)
    {
        if (whichHand)
        {
            rTriggerDown = pressure < .1f ? false : true;
            //Debug.Log("rTriggerDown changed to " + rTriggerDown);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("something touched the dial");

        if (other.gameObject.CompareTag("Right Hand"))
        {
            print("hand found, attached to dial");
            //assign gameObject to hand
            hand = other.gameObject;
            Debug.Log(hand.name);

            whichHand = true;
        }
        else if (other.gameObject.CompareTag("Left Hand"))
        {
            print("hand found, attached to dial");
            //assign gameObject to hand
            hand = other.gameObject;
            Debug.Log(hand.name);

            whichHand = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        print("something stopped touching the dial");
        
        if(other.gameObject == hand)
		{
            hand = null;
        }
    }
}

