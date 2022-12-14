using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class LaserTag : MonoBehaviour
{
    public enum markers
    {
        MovedObject,
        Radiation,
        Flammable,
        Corrosion,
        Caution
    }

    public GameObject[] prefabs;
    [SerializeField] markers marker = markers.Caution;
    SteamVR_LaserPointer laserPointer;
    List<GameObject> placedMarkers;
    // Start is called before the first frame update
    void Start()
    {
        laserPointer = FindObjectOfType<SteamVR_LaserPointer>();
        laserPointer.PointerClick += PointerClick;
        placedMarkers = new List<GameObject>();
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Transform handTransform = laserPointer.gameObject.transform;
        Ray raycast = new Ray(handTransform.position, handTransform.forward);
        RaycastHit hit;        
        bool bHit = Physics.Raycast(raycast, out hit);

        if (e.target.tag == "marker")//assuming all markers have the marker tag, if one is clicked
        {
            placedMarkers.Remove(e.target.gameObject);//remove it from the list TODO decide if a list of markers is needed
            Destroy(e.target.gameObject);//destroy it
        }
        else
        {
            placedMarkers.Add(Instantiate(prefabs[(int)marker], hit.point, prefabs[(int)marker].transform.rotation));//TODO check if it spawns at the right height
        }
    }

    public void SetMarker(int m)
    {
        marker = (markers)m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
