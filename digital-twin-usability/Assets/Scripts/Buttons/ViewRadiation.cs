using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRadiation : MonoBehaviour
{
    public GameObject radiationLayer;

    public void Start()
    {


    }

    public void SetText()
    {

        Debug.Log("Trigger Radiation View!");

        if (radiationLayer.activeSelf == false)
        {
            radiationLayer.SetActive(true);
        }
        else
        {
            radiationLayer.SetActive(false);
        }
    }

}
