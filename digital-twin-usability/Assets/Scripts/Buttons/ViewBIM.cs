using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBIM : MonoBehaviour
{
    public GameObject bimGroup;

    public void GetInput()
    {

    }

    public void SetText()
    {

        Debug.Log("Trigger BIM View!");

        if (bimGroup.activeSelf == false)
        {
            bimGroup.SetActive(true);
        }
        else
        {
            bimGroup.SetActive(false);
        }
    }

}
