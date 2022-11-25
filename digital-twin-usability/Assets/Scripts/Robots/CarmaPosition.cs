using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using carma_pos = RosMessageTypes.Geometry.TransformStampedMsg;

public class CarmaPosition : MonoBehaviour
{
    public GameObject carma_model;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<carma_pos>("vicon/CARMA_Sprint/CARMA_Sprint", move_carma);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void move_carma(RosMessageTypes.Geometry.TransformStampedMsg trans)
    {
        carma_model.transform.localPosition = new Vector3(((float)trans.transform.translation.x), ((float)trans.transform.translation.z), ((float)trans.transform.translation.y));
        //Debug.Log(trans);
        carma_model.transform.localRotation = new Quaternion(((float)trans.transform.rotation.w), ((float)trans.transform.rotation.x), ((float)trans.transform.rotation.z), ((float)trans.transform.rotation.y));
    }
}
