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
    public TriggerEvent triggerPress;
    public TriggerEvent triggerRelease;

    private bool lastButtonState = false;
    private List<InputDevice> inputDevices;

    //private InputDevice headsetDevice;
    private Camera playerCam;
    //MotionControllerStateCache moConCache;

    bool triggerDownLastState = false;
    [HideInInspector] public delegate void triggerAction(float pressure);
    [HideInInspector] public static event triggerAction triggerLeft;
    [HideInInspector] public static event triggerAction triggerRight;

    private InputDevice leftWand;
    private InputDevice rightWand;
    //private InputDevice hmd;

    private void Awake() {
        if (triggerPress == null) {
            triggerPress = new TriggerEvent();
            triggerRelease = new TriggerEvent();
        }

        inputDevices = new List<InputDevice>();

        playerCam = FindObjectOfType<Camera>(); //look for player camera in scene
        //if(playerCam)
            //print("Player camera found");
    }

    void OnEnable() {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices) {
            InputDevices_deviceConnected(device);
		}

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable() {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        inputDevices.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device) {
        print("Device connected! " + device.name);

        //look at characteristic bitmasks to see what this controller is for
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
        
        leftWand.TryGetFeatureValue(CommonUsages.secondary2DAxis, out leftJoyVal);

        if (leftWand.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerVal))
        {
            if (triggerLeft != null)
                triggerLeft(leftTriggerVal);
        }
        if (rightWand.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerVal))
        { 
            if (triggerRight != null)
                triggerRight(rightTriggerVal);
        }

        //getting the direct headset rotation is unnecessarily difficult so i'm gonna do this in a jank way
        //just get the y rotation of the camera attached to the headset and apply that to the player

        float playerCamY = playerCam.transform.rotation.eulerAngles.y; //get hmd rotation from player camera
        float speed = 1f;
        Vector3 movement = new Vector3(leftJoyVal.x, 0, leftJoyVal.y) * speed;
        movement = Quaternion.Euler(0, playerCamY, 0) * movement; // apply y rotation of hmd to movement vector
        if (movement.magnitude > 0.5f) transform.position += movement * Time.deltaTime; //apply movement to character (w/ deadzone) 
    }
}
