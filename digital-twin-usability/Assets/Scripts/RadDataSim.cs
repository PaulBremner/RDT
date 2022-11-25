using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadDataSim : MonoBehaviour
{

    List<float[]> radData;
    RadMeshCreator radMeshCreator;

    // Start is called before the first frame update
    void Start()
    {
        radData = new List<float[]>();

        for(int i = 0; i<100; i++)
        {
            float[] r = new float[] { i/100f, 0, 0, i / 100f };
            Debug.Log(r[0]);
            radData.Add(r);
        }

        radMeshCreator = FindObjectOfType<RadMeshCreator>();//find an instance of the mesh creation script

        radMeshCreator.CreateRadMesh(radData);//call with list of radiation data, each call creates a new mesh with new data so don't call it too often
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
