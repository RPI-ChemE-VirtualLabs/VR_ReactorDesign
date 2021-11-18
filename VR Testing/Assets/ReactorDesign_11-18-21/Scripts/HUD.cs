using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject HUDisplay;
    public float PushInDistance = 0.01f;
    public bool Halt = false;
    public AudioSource Click;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Interactable>().isInteractable = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameObject.GetComponent<Interactable>().isInteractable == true) 
        {
            gameObject.GetComponent<Interactable>().isInteractable = false;
            if (Halt == false)
            {
                Halt = true;

                gameObject.GetComponent<Interactable>().isInteractable = false;
                transform.localPosition += Vector3.down * PushInDistance * transform.parent.transform.localScale.y;
                Click.Play();
                HUDisplay.active = !HUDisplay.active;

                StartCoroutine("FreezingTime");
            }
        }
    }
    IEnumerator FreezingTime()
    {
        gameObject.GetComponent<Interactable>().isInteractable = false;
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Wait for 3 seconds");
        transform.localPosition += Vector3.up * PushInDistance * transform.parent.transform.localScale.y;
        Halt = false;
        gameObject.GetComponent<Interactable>().isInteractable = false;
    }
}
