using Microsoft.MixedReality.Toolkit.XRSDK.WindowsMixedReality;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconView : MonoBehaviour
{
    float height;
    RectTransform rt;
    [HideInInspector] public TextMeshPro text;
    public bool windowActive { get; protected set; } = false;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        GameObject txtObj = GameObject.Find("Econ View Text");
        text = txtObj.GetComponent<TextMeshPro>();
    }

    public void OnOpen()
    {
        StopCoroutine("CloseWindow");
        StartCoroutine("OpenWindow");
        windowActive = true;
    }
    
    public void OnClose()
    {
        StopCoroutine("OpenWindow");
        StartCoroutine("CloseWindow");
        windowActive = false;
    }

    IEnumerator OpenWindow()
    {
        float rate = 3;
        //Debug.Log(rt.localScale.y);
        while (rt.localScale.y < 1)
        {
            rt.localScale = new Vector3(1, Mathf.Lerp(rt.localScale.y, 1, Time.deltaTime * rate), 1);
            yield return null;
        }
        yield return null;
    }
    
    IEnumerator CloseWindow()
    {
        float rate = 3;
        //Debug.Log(rt.localScale.y);
        while (rt.localScale.y > 0)
        {
            rt.localScale = new Vector3(1, Mathf.Lerp(rt.localScale.y, 0, Time.deltaTime * rate), 1);
            yield return null;
        }
        yield return null;
    }
}
