using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerDebug : MonoBehaviour
{
    // Recolor objects that we manipulate.
    [SerializeField] Color handPresenceIndicator;
    [SerializeField] float speed = 1;
    [SerializeField] float sensitivity = 2.5f;
    private CharacterController control;
    public static bool usingDebug
    {
        get;
        protected set;
    } = false;
    // Start is called before the first frame update

    public delegate void TriggerDown(float pressure);
    public static event TriggerDown debugTriggerDown;
    public delegate void TriggerUp(float pressure);
    public static event TriggerUp debugTriggerUp;

    private Camera mainCam;
    private GameObject currentSelection;
    void Start()
    {
        Debug.LogWarning("Using debug character controller.");
        control = GetComponent<CharacterController>();
        mainCam = transform.GetChild(0).GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        usingDebug = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        OnMovement(input);

        Vector2 mouseInput = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));
        OnMouse(mouseInput);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            OnClick();

        if (Input.GetKeyDown(KeyCode.E) && debugTriggerDown != null)
            debugTriggerDown(1f);
        else if (Input.GetKeyUp(KeyCode.E) && debugTriggerUp != null)
            debugTriggerUp(1f);
    }

    private void OnMovement(Vector2 input)
	{
        Vector3 mv = new Vector3(input.x, 0, input.y);
        mv = transform.localRotation * mv;
        control.SimpleMove(mv * speed);
	}

    private float pitch = 0;
    private float yaw = 0;

	private void OnMouse(Vector2 mouseInput)
	{
        yaw += mouseInput.x;
        pitch -= mouseInput.y;
        transform.eulerAngles = new Vector3(pitch, yaw, 0);
	}

    private void OnClick()
	{
        RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.position + mainCam.ScreenPointToRay(Screen.currentResolution.));
        if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
		{
            if(hit.collider.tag == "VR Object")
			{
                currentSelection = hit.collider.gameObject;
                //oldColor = currentSelection.GetComponent<Material>().color;
                currentSelection.GetComponent<Renderer>().material.SetColor("_Color", handPresenceIndicator);

                debugTriggerDown += currentSelection.GetComponent<VRButton>().OnVRTriggerDown; 
                debugTriggerUp += currentSelection.GetComponent<VRButton>().OnVRTriggerUp; 
			}
            //Debug.Log(hit.collider.gameObject.name);

		}
	}
}
