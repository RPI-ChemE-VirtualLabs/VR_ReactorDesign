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
public class PrimaryButtonEvent : UnityEvent<bool> { }

public class VR_CharacterController : MonoBehaviour {
    public PrimaryButtonEvent primaryButtonPress;

    private bool lastButtonState = false;
    private List<InputDevice> inputDevices;

    //private InputDevice headsetDevice;
    private Camera playerCam;
    MotionControllerStateCache moConCache;

    private void Awake() {
        if (primaryButtonPress == null) {
            primaryButtonPress = new PrimaryButtonEvent();
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
        /*if (device.characteristics == InputDeviceCharacteristics.HeadMounted) {
            headsetDevice = device;
            print("HMD found! " + device.name);
        }*/
        inputDevices.Add(device); 
    }

    private void InputDevices_deviceDisconnected(InputDevice device) {
        if (inputDevices.Contains(device))
            inputDevices.Remove(device);
    }

    void Update() {
        bool tempState = false;
        print("dev with primary: " + inputDevices.Count);
        //todo: figure out how to actually find the controllers. these headsets don't seem to have any characteristics

        //don't try to read input through unity anymore. wmr has its own thing:
        //https://docs.microsoft.com/en-us/windows/mixed-reality/develop/unity/unity-reverb-g2-controllers

        foreach (var device in inputDevices) {
            bool gripButtonState = false;
            Vector2 joystickValue = Vector2.zero;
            tempState = device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out joystickValue); // did get a value

            print(device.name);
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

        if (tempState != lastButtonState) { // Button state changed since last frame
            primaryButtonPress.Invoke(tempState);
            lastButtonState = tempState;
        }
    }

    public void ButtonPressed(bool pressed) {
        print("WOO HOOO BUTTON PRESSED: " + pressed);
	}
}
