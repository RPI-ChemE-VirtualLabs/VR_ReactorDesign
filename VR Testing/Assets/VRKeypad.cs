using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRKeypad : MonoBehaviour
{
    public TextMeshPro tm;
    public float currentValue = 0;
    private float tempValue = 0;
    // Start is called before the first frame update

    // use VRButtons for key pressing logic, this class for handling values
    public void EnterValue(int val)
    {
        if (val == -1)
            tempValue = 0;
        else if (val == -2)
            currentValue = tempValue;
        else
        {
            tempValue *= 10;
            tempValue += val;
        }

        tm.text = tempValue.ToString();
    }
}
