using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    GameObject hand = null;
    bool whichHand = false; // false = left, true = right
    bool handInRange = false;
    bool lTriggerDown = false;
    bool rTriggerDown = false;

    void Awake()
    {
        VR_CharacterController.triggerLeft += OnVRTrigger;
        VR_CharacterController.triggerRight += OnVRTrigger;
    }

	public virtual void OnVRTrigger(float pressure)
	{
        if (pressure != 0)
            transform.position = new Vector3(0, .5f, 0);
        else
            transform.position = new Vector3(0, 1.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("something touched the button");

        if (other.gameObject.CompareTag("Right Hand"))
        {
            print("hand found, attached to button");
            //assign gameObject to hand
            hand = other.gameObject;
            Debug.Log(hand.name);

            whichHand = true;
        }
        else if (other.gameObject.CompareTag("Left Hand"))
        {
            print("hand found, attached to button");
            //assign gameObject to hand
            hand = other.gameObject;
            Debug.Log(hand.name);

            whichHand = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("something stopped touching the button");

        if (other.gameObject == hand)
        {
            hand = null;
        }
    }
}
