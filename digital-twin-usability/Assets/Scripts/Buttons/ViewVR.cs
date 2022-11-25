using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVR : MonoBehaviour
{
    public GameObject VRCamera;
    public GameObject FlatScreenCamera;

    public void Start()
    {
        FlatScreenCamera.SetActive(true);
        VRCamera.SetActive(false);

    }

    public void SetText()
    {

        if (VRCamera.activeSelf == false)
        {
            Debug.Log("Trigger VR View!");
            VRCamera.SetActive(true);
            FlatScreenCamera.SetActive(false);
        }
        else
        {
            Debug.Log("Trigger Flat Screen View!");
            VRCamera.SetActive(false);
            FlatScreenCamera.SetActive(true);
        }
    }

}
