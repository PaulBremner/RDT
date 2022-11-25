using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    public GameObject cameraToLookAtFlat;
    public GameObject cameraToLookAtVR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraToLookAtVR.activeSelf == false)
        {
            // Camera to look at is Flat Screen Cam
            Vector3 v = cameraToLookAtFlat.transform.position - transform.position;

            v.x = v.z = 0.0f;
            transform.LookAt(cameraToLookAtFlat.transform.position - transform.position - v);
            transform.rotation = (cameraToLookAtFlat.transform.rotation);

        }
        else
        {
            // Camera to look at is VR Cam
            Vector3 v = cameraToLookAtVR.transform.position - transform.position;

            v.x = v.z = 0.0f;
            transform.LookAt(cameraToLookAtVR.transform.position - transform.position - v);
            transform.rotation = (cameraToLookAtVR.transform.rotation);
        }

              
    }
}
