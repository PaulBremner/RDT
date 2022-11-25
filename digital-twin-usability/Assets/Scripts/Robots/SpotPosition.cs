using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using spot_pos = RosMessageTypes.Geometry.TransformStampedMsg;

public class SpotPosition : MonoBehaviour
{
    public GameObject spot_model; 

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<spot_pos>("vicon/spot/spot", move_spot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void move_spot(RosMessageTypes.Geometry.TransformStampedMsg trans)
    {
        spot_model.transform.localPosition = new Vector3(((float)trans.transform.translation.x), ((float)trans.transform.translation.z), ((float)trans.transform.translation.y));
        //Debug.Log(trans);
        spot_model.transform.localRotation = new Quaternion(((float)trans.transform.rotation.w), ((float)trans.transform.rotation.x), ((float)trans.transform.rotation.z), ((float)trans.transform.rotation.y));
    }
}
