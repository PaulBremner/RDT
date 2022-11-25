using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSensors : MonoBehaviour
{
    public GameObject defaultVizSuite;

    public void Start()
    {


    }

    public void SetText()
    {

        Debug.Log("Trigger Sensor View!");

        if (defaultVizSuite.activeSelf == false)
        {
            defaultVizSuite.SetActive(true);
        }
        else
        {
            defaultVizSuite.SetActive(false);
        }
    }

}
