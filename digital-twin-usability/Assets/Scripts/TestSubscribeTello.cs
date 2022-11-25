using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine.UI;
using tello_msg = RosMessageTypes.Std.StringMsg;
using tello_img = RosMessageTypes.Sensor.CompressedImageMsg;


public class TestSubscribeTello : MonoBehaviour
{
    public RawImage img;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<tello_img>("tello/image_raw/h264", detect);
    }

    void detect(RosMessageTypes.Sensor.CompressedImageMsg message)
    {
        Debug.Log(message);
        //img.texture = message;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
