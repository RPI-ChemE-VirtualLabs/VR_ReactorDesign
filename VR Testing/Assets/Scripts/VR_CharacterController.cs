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

    private InputDevice headsetDevice;

    private void Awake() {
        if (primaryButtonPress == null) {
            primaryButtonPress = new PrimaryButtonEvent();
        }

        inputDevices = new List<InputDevice>();
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
        if (device.role == InputDeviceRole.Generic) {
            headsetDevice = device;

		}
        inputDevices.Add(device); 
    }

    private void InputDevices_deviceDisconnected(InputDevice device) {
        if (inputDevices.Contains(device))
            inputDevices.Remove(device);
    }

    void Update() {
        bool tempState = false;
        print("dev with primary: " + inputDevices.Count);
        foreach (var device in inputDevices) {
            bool gripButtonState = false;
            Vector2 joystickValue = Vector2.zero;
            tempState = device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out joystickValue); // did get a value

            print(device.name);
            print("js value: " + joystickValue);

            Quaternion eyeRotation = Quaternion.identity;
            float horizontalRotation = eyeRotation.eulerAngles.y;

            headsetDevice.TryGetFeatureValue(CommonUsages.centerEyeRotation, out eyeRotation);

            float speed = 1f;
            Vector3 movement = new Vector3(joystickValue.x, 0f, joystickValue.y) * speed;
            Vector3 orientedMovement = Quaternion.AngleAxis(horizontalRotation, Vector3.up) * movement;
            print(orientedMovement);

            if (orientedMovement.magnitude > 0.1f) transform.position += orientedMovement * Time.deltaTime;

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
