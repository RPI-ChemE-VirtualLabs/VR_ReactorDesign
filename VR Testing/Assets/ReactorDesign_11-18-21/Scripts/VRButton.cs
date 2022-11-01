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
    public bool buttonActive = false;

    [Header("Button Aesthetics")]
    Vector3 restPos;
    [SerializeField] float pressDepth;

    public virtual void Awake()
    {
        VR_CharacterController.triggerLeft += OnVRTrigger;
        VR_CharacterController.triggerRight += OnVRTrigger;

        restPos = transform.position;
    }

    //override this guy in child classes
	public virtual void OnVRTrigger(float pressure)
	{
        if (hand && pressure != 0)
        {
            transform.position = restPos - transform.up * pressDepth;
            buttonActive = true;
        }
        else
		{
            transform.position = restPos;
            buttonActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("something touched the button");

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
