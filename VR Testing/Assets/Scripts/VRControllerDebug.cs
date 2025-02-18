using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerDebug : MonoBehaviour
{
    // Recolor objects that we manipulate.
    [SerializeField] Color handPresenceIndicator;
    private Color m_oldColor;
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
    private EconView ev;
    void Start()
    {
        Debug.LogWarning("Using debug character controller.");
        control = GetComponent<CharacterController>();
        mainCam = transform.GetChild(0).GetComponent<Camera>();
        ev = GameObject.Find("Econ View Window").GetComponent<EconView>();
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ev.windowActive)
                ev.OnClose();
            else
                ev.OnOpen();
        }

        if (Input.GetKeyDown(KeyCode.E) && debugTriggerDown != null)
            debugTriggerDown(1f);
        else if (Input.GetKeyUp(KeyCode.E) && debugTriggerUp != null)
            debugTriggerUp(1f);
    }

    private void OnMovement(Vector2 input)
	{
        Vector3 mv = new Vector3(input.x, 0, input.y);
        mv = (mainCam.transform.localRotation * Quaternion.Euler(Vector3.up * 90)) * mv;
        control.SimpleMove(mv * speed);
	}

    private float pitch = 0;
    private float yaw = 0;

	private void OnMouse(Vector2 mouseInput)
	{
        yaw += mouseInput.x;
        pitch -= mouseInput.y;
        mainCam.transform.eulerAngles = new Vector3(pitch, yaw, 0);
	}

    private void OnClick()
	{
        RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.position + mainCam.ScreenPointToRay(Screen.currentResolution.));
        if(Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
		{
            if(hit.collider.tag == "VR Object")
			{
                // If there is already an object selected, clear any references
                // to it and remove input events from the delegates.
				if (currentSelection)
				{
                    currentSelection.GetComponent<Renderer>().material.SetColor("_Color", m_oldColor);
                    if(debugTriggerDown != null)
					    debugTriggerDown -= currentSelection.GetComponent<VRButton>().OnVRTriggerDown; 
                    if(debugTriggerUp != null)
						debugTriggerUp -= currentSelection.GetComponent<VRButton>().OnVRTriggerUp; 
				}

                // Get object references and change object color.
                currentSelection = hit.collider.gameObject;
                m_oldColor = currentSelection.GetComponent<Renderer>().material.color;
                currentSelection.GetComponent<Renderer>().material.SetColor("_Color", handPresenceIndicator);

                // Add VR input events to delegates.
                debugTriggerDown += currentSelection.GetComponent<VRButton>().OnVRTriggerDown; 
                debugTriggerUp += currentSelection.GetComponent<VRButton>().OnVRTriggerUp; 
			}
		}
	}
}
