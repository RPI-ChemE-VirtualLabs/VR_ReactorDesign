using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    // TODO: Abstract this class further.
    protected GameObject hand = null;
    enum Hand {NONE, LEFT, RIGHT };
    Hand currentHand = Hand.NONE; 
    bool handInRange = false;
    bool active = false;
    bool buttonDown = false;
    public bool Active {
        get { return active; } 
        protected set { active = value; } 
    }

    [Header("Button Aesthetics")]
    protected Vector3 restPos;
    [SerializeField] protected float pressDepth;

    public virtual void Awake()
    {
        // Register this object with the character controller.
        VR_CharacterController.triggerLeftDown += OnVRTriggerDown;
        VR_CharacterController.triggerRightDown += OnVRTriggerDown;
        VR_CharacterController.triggerLeftUp += OnVRTriggerUp;
        VR_CharacterController.triggerRightUp += OnVRTriggerUp;

        restPos = transform.position;

        // Give this object a VR object tag if it doesn't have it already.
        if (gameObject.CompareTag("Untagged")) {
            Debug.LogWarning("Untagged object was given the \"VR Object\" tag.", this);
            gameObject.tag = "VR Object";
        }
    }

    // Fire when a controller's trigger is pressed down.
    /* Parameters:
     *  pressure: float between 0 (not pressed at all) and 1 (completely pressed)
    */
	public virtual void OnVRTriggerDown(float pressure)
	{
        // If this object is being touched and the button isn't already down...
        if (hand != null && !buttonDown)
        {
            // Push the object down and mark it as active.
            transform.position = restPos - transform.up * pressDepth;
            Active = true;
            Debug.Log("Changed.");
            buttonDown = true;
        }
    }

    // Fire when a controller's trigger is being released.
	public virtual void OnVRTriggerUp(float pressure)
	{
        // Reset position and mark as inactive.
        if (buttonDown)
        {
            transform.position = restPos;
            Active = false;
            buttonDown = false;
        }
        // Debug.Log("Trigger released.");
	}

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a hand (and which hand it is),
        // and get a reference to it.
        // TODO: This doesn't need to be an if/else.
        if (other.gameObject.CompareTag("Right Hand"))
        {
            print("hand found, attached to button");
            hand = other.gameObject;
            Debug.Log(hand.name);

            currentHand = Hand.RIGHT;
        }
        else if (other.gameObject.CompareTag("Left Hand"))
        {
            print("hand found, attached to button");
            hand = other.gameObject;
            Debug.Log(hand.name);

            currentHand = Hand.LEFT;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the hand leaves the collider, clear the reference.
        if (other.gameObject == hand)
        {
            hand = null;
        }
    }
}
