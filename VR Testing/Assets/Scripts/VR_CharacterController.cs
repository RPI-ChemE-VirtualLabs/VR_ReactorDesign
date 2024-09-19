using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

/*
public class VR_CharacterController : MonoBehaviour {
    string leftControllerName = "Spatial Controller - Left"; // role: LeftHanded
    string rightControllerName = "Spatial Controller - Right"; // role: RightHanded
    string headsetName =  "WindowsMR Headset(HP Reverb VR Headset VR1000-2xxx)"; // role: Generic

    void Update() {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        print("start-------------------------------------------");
        print("finish-------------------------------------------");
    }
}*/

[System.Serializable]
public class TriggerEvent : UnityEvent<bool> { }

public class VR_CharacterController : MonoBehaviour {
    //public TriggerEvent triggerPress;
    //public TriggerEvent triggerRelease;

    private bool lastButtonState = false;
    private List<InputDevice> inputDevices;

    //private InputDevice headsetDevice;
    private Camera playerCam;
    //MotionControllerStateCache moConCache;

    bool triggerDownLastState = false;
    // TODO: Change case of triggerAction
    [HideInInspector] public delegate void triggerAction(float pressure);
    [HideInInspector] public static event triggerAction triggerLeftDown;
    [HideInInspector] public static event triggerAction triggerRightDown;
    [HideInInspector] public static event triggerAction triggerLeftUp;
    [HideInInspector] public static event triggerAction triggerRightUp;

    private InputDevice leftWand;
    private InputDevice rightWand;
    //private InputDevice hmd;

    private void Awake() {
        // Register events.
        /*
        if (triggerPress == null) {
            triggerPress = new TriggerEvent();
            triggerRelease = new TriggerEvent();
        }*/

        inputDevices = new List<InputDevice>();

        playerCam = FindObjectOfType<Camera>(); //look for player camera in scene
        //if(playerCam)
            //print("Player camera found");
    }

    void OnEnable() {
        // Look for connected devices and register them.
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices) {
            InputDevices_deviceConnected(device);
		}

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable() {
        // Unregister inputs that need to be disconnected.
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        inputDevices.Clear();
    }

    // Registers devices based on function (e.g. distinguishing HMD vs Wand)
    private void InputDevices_deviceConnected(InputDevice device) {
        Debug.Log("Device connected! " + device.name);

        inputDevices.Add(device);
        if((device.characteristics & InputDeviceCharacteristics.HeldInHand) == InputDeviceCharacteristics.HeldInHand)
		{
            if((device.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
			{
                Debug.Log("Left wand found");
                leftWand = device;
            }
            else if((device.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
			{
                Debug.Log("Right wand found");
                rightWand = device;
			}
		}

        /*
        if((device.characteristics & InputDeviceCharacteristics.HeadMounted) == InputDeviceCharacteristics.HeadMounted)
		{
            Debug.Log("HMD found");
            hmd = device;
		}
        */
    }

    private void InputDevices_deviceDisconnected(InputDevice device) {
        if (inputDevices.Contains(device))
            inputDevices.Remove(device);
    }

    void Update() {
        //todo: right controller specifically is drifting forward (checked with both controllers, not hw issue), why?
        //use system of booleans to get input states

        //go through each device to grab input
        float leftTriggerVal;
        float rightTriggerVal;
        Vector2 leftJoyVal;
        //Vector2 rightJoyVal;
        
        // Get state of left stick for movement.
        // TODO: Move character movement to another script and only handle input here.
        leftWand.TryGetFeatureValue(CommonUsages.secondary2DAxis, out leftJoyVal);

        if (leftWand.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerVal))
        {
            if (triggerLeftDown != null && leftTriggerVal > 0)
                triggerLeftDown(leftTriggerVal);
            else if(triggerLeftUp != null)
                triggerLeftUp(leftTriggerVal);
        }
        if (rightWand.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerVal))
        { 
            if (triggerRightDown != null && rightTriggerVal > 0)
                triggerRightDown(rightTriggerVal);
            else if(triggerRightUp != null)
                triggerRightUp(leftTriggerVal);
        }

        //getting the direct headset rotation is unnecessarily difficult so i'm gonna do this in a jank way
        //just get the y rotation of the camera attached to the headset and apply that to the player
        //Debug.Log(leftJoyVal);

        float playerCamY = playerCam.transform.rotation.eulerAngles.y; //get hmd rotation from player camera
        float speed = 1f;
        Vector3 movement = new Vector3(leftJoyVal.x, 0, leftJoyVal.y) * speed;
        movement = Quaternion.Euler(0, playerCamY, 0) * movement; // apply y rotation of hmd to movement vector
        //Debug.Log(movement);
        if (movement.magnitude > 0.5f) 
            transform.position += movement * Time.deltaTime; //apply movement to character (w/ deadzone) 
    }
}
