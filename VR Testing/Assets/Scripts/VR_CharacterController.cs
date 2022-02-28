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

    private bool lastButtonState = false;
    private List<InputDevice> inputDevices;

    //private InputDevice headsetDevice;
    private Camera playerCam;
    MotionControllerStateCache moConCache;

    private void Awake() {
        if (triggerPress == null) {
            triggerPress = new TriggerEvent();
        }

        inputDevices = new List<InputDevice>();

        playerCam = FindObjectOfType<Camera>(); //look for player camera in scene
        if(playerCam)
            print("Player camera found");
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

        //bitwise comparisons like these are used to determine if a device has a given characteristic
        //looks kinda janky but this is how it works in the api
        if ((device.characteristics & InputDeviceCharacteristics.HeadMounted) == InputDeviceCharacteristics.HeadMounted) {
            //headsetDevice = device;
            print("HMD found! " + device.name);
        }
        inputDevices.Add(device);

        //at some point i'd like to have devices assigned to specific "roles" rather than a vague list of devices
        //that way we don't have to cycle through every device we need in update, plus we can assign different actions to each hand
        Debug.Log(device.characteristics);
    }

    private void InputDevices_deviceDisconnected(InputDevice device) {
        if (inputDevices.Contains(device))
            inputDevices.Remove(device);
    }

    void Update() {
        //todo: right controller specifically is drifting forward (checked with both controllers, not hw issue), why?
        //use system of booleans to get input states
        bool triggerDown = false;

        //go through each device to grab input
        foreach (var device in inputDevices) {
            Vector2 joystickValue = Vector2.zero;

            device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out joystickValue);
            device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerDown);

         
            //print(device.name);
            //print("js value: " + joystickValue);

            //getting the direct headset rotation is unnecessarily difficult so i'm gonna do this in a jank way
            //just get the y rotation of the camera attached to the headset and apply that to the player

            float playerCamY = playerCam.transform.rotation.eulerAngles.y; //get hmd rotation from player camera

            float speed = 1f;
            Vector3 movement = new Vector3(joystickValue.x, 0f, joystickValue.y) * speed; //apply speed to movement vector
            //print(movement);
            //print("hmd rotation: " + playerCamY);
            movement = Quaternion.Euler(0, playerCamY, 0) * movement; // apply y rotation of hmd to movement vector
            if (movement.magnitude > 0.1f) transform.position += movement * Time.deltaTime; //apply movement to character (w/ deadzone) 
        }

        if (triggerDown) //trigger event based on input state
            triggerPress.Invoke(triggerDown);
    }
}
