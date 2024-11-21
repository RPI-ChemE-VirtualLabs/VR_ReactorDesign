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
    private bool lastButtonState = false;
    private List<InputDevice> inputDevices;

    private Camera playerCam;

    bool triggerDownLastState = false;
    // TODO: Change case of triggerAction
    [HideInInspector] public delegate void triggerAction(float pressure);
    [HideInInspector] public static event triggerAction triggerLeftDown;
    [HideInInspector] public static event triggerAction triggerRightDown;
    [HideInInspector] public static event triggerAction triggerLeftUp;
    [HideInInspector] public static event triggerAction triggerRightUp;

    [HideInInspector] public delegate void menuAction();
    [HideInInspector] public static event menuAction menuDown;
    [HideInInspector] public static event menuAction menuUp;
    [HideInInspector] public static event menuAction menuDownTriple;

    private InputDevice leftWand;
    private InputDevice rightWand;

    // Time measurement to detect double/triple clicks.
    bool buttonTimerActive;
    float buttonTimer = 0;
    float buttonTimerMax = 1.0f;
    int numClicks = 0;

    private void Awake() {
        inputDevices = new List<InputDevice>();

        playerCam = FindObjectOfType<Camera>(); 
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
        float leftTriggerVal;
        float rightTriggerVal;
        Vector2 leftJoyVal;
        Vector2 rightJoyVal;
        bool leftMenu = false;
        
        // Get state of left stick for movement.
        // TODO: Move character movement to another script and only handle input here.
        leftWand.TryGetFeatureValue(CommonUsages.secondary2DAxis, out leftJoyVal);
        rightWand.TryGetFeatureValue(CommonUsages.secondary2DAxis, out rightJoyVal);

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

        // Get menu button state.
        if (leftWand.TryGetFeatureValue(CommonUsages.menuButton, out leftMenu))
		{
			if (menuDown != null )
			{
                if (leftMenu)
                {
                    menuDown();
					if (!buttonTimerActive)
					{
                        buttonTimerActive = true;
					}
                    numClicks++;
                    if(numClicks >= 3 && menuDownTriple != null)
					{
                        menuDownTriple();
                        numClicks = 0;
                        buttonTimerActive = false;
					}
                }
			}
		}

        if(buttonTimerActive)
		{
            buttonTimer += Time.deltaTime;
		}

        //getting the direct headset rotation is unnecessarily difficult so i'm gonna do this in a jank way
        //just get the y rotation of the camera attached to the headset and apply that to the player
        //Debug.Log(leftJoyVal);

        float playerCamY = playerCam.transform.rotation.eulerAngles.y; //get hmd rotation from player camera
        float speed = 1f;
        Vector3 left_movement = new Vector3(leftJoyVal.x, 0, leftJoyVal.y) * speed;
        Vector3 right_movement = new Vector3(rightJoyVal.x, 0, rightJoyVal.y) * speed;
        Vector3 movement = left_movement.magnitude > right_movement.magnitude ? left_movement : right_movement;
        movement = Quaternion.Euler(0, playerCamY, 0) * movement; // apply y rotation of hmd to movement vector
        //Debug.Log(movement);
        if (movement.magnitude > 0.5f) 
            transform.position += movement * Time.deltaTime; //apply movement to character (w/ deadzone) 
    }
}
