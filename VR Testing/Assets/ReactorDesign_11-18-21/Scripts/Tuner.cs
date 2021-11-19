using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Tuner : MonoBehaviour
{
    public float Percentage = 0f;

    public GameObject Character;
    public Material stairprops;
    public GameObject ResetCFTButton;
    
    [SerializeField]
    float sensitivity = .001f;
    [SerializeField]
    float sensitivityChangeDivide = 20;
    bool sensitivityChange = false;

    GameObject hand;
    bool handInRange = false;

    List<InputDevice> playerControllers = new List<InputDevice>();
    //InputDevices.GetDevicesWithRole(InputDeviceRole.GameController, gameControllers);

    // Start is called before the first frame update
    void Start()
    {
        InputDevices.GetDevices(playerControllers);
        foreach (var device in playerControllers) {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Interactable>().isInteractable == true)
        {
            //Cursor.lockState = CursorLockMode.None;
            float addPercent = 0;// = -Input.GetAxis("Mouse X") * sensitivity;

            Percentage += addPercent;

            Percentage = Mathf.Clamp(Percentage, -1, 1);

            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(-90, 0, 140 * -Percentage - 90));

            Debug.Log("Mouse pos: " + Input.mousePosition.x);
            //Debug.Log(Percentage); 

            if (Input.GetMouseButtonUp(0))
            {
                gameObject.GetComponent<Interactable>().isInteractable = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            //press shift to lower percentage change sensitivity (toggle to make it consistent)
            if (Input.GetKeyDown(KeyCode.LeftShift)) 
            {
                if (!sensitivityChange)
                {
                    sensitivity /= sensitivityChangeDivide;
                    sensitivityChange = true;
                }
                else
                {
                    sensitivity *= sensitivityChangeDivide;
                    sensitivityChange = false;
                }
            }
        }
    }

	private void OnTriggerEnter(Collider other) {
        print("something touched the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            //assign gameObject to hand
            hand = other.gameObject;
            print("hand found, attached to dial");

            handInRange = true; //enable selection
        }
    }

	private void OnTriggerExit(Collider other) {
        print("something stopped touching the dial");

        if (other.gameObject.CompareTag("Hand")) //check if hand
        {
            handInRange = false; //disable selection
        }
    }
}
