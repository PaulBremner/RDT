using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateRotate : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 25f, 0f) * Time.deltaTime);
        
    }
}
