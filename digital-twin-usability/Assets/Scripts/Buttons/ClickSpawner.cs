using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Code by SharpCoderBlog.com

public class ClickSpawner : MonoBehaviour
{
    public GameObject[] prefabs; //Prefabs to spawn

    public Camera camera;
    int selectedPrefab = 0;
    int rayDistance = 300;

    // For GUI
    string taggingTool = "No Tool Selected";
    int fontsize = 20;

    // Instantiate once
    bool spawnerActive;

    // Hazard codes
    int hazardCounter = 0;
    string[] hazardCodes = {"Moved Object(s)", "Radiation", "Flammable", "Corrosion/Caustic", "Caution"};
    string hazardCode;

    // Report summary
    public TextMeshProUGUI displayReport;

    // Start is called before the first frame update
    void Start()
    {
        spawnerActive = false;

        if (prefabs.Length == 0)
        {
            Debug.LogError("You haven't assigned any Prefabs to spawn");
        }

        displayReport.GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnerActive = true;
            selectedPrefab++;
            if (selectedPrefab >= prefabs.Length)
            {
                selectedPrefab = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnerActive = true;
            selectedPrefab--;
            if (selectedPrefab < 0)
            {
                selectedPrefab = prefabs.Length - 1;
            }
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            //Remove spawned prefab when holding left shift and left clicking
            Transform selectedTransform = GetObjectOnClick();
            if (selectedTransform)
            {
                Destroy(selectedTransform.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            //On left click spawn selected prefab and align its rotation to a surface normal
            Vector3[] spawnData = GetClickPositionAndNormal();
            if (spawnData[0] != Vector3.zero && spawnerActive == true)
            {
                GameObject go = Instantiate(prefabs[selectedPrefab], spawnData[0], Quaternion.FromToRotation(prefabs[selectedPrefab].transform.up, spawnData[1]));
                go.name += " _instantiated";

                hazardCounter++;
                hazardCode = hazardCounter.ToString() + "-" + hazardCodes[selectedPrefab];
                Debug.Log(hazardCode);

                displayReport.text = hazardCode;
            }

            spawnerActive = false;
        }
    }

    Vector3[] GetClickPositionAndNormal()
    {
        Vector3[] returnData = new Vector3[] { Vector3.zero, Vector3.zero }; //0 = spawn poisiton, 1 = surface normal
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            returnData[0] = hit.point;
            returnData[1] = hit.normal;
        }

        return returnData;
    }

    Transform GetObjectOnClick()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Transform root = hit.transform.root;
            if (root.name.EndsWith("_instantiated"))
            {
                return root;
            }
        }

        return null;
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = fontsize;
        GUI.color = new Color(1, 1, 1, 1);
        GUI.Label(new Rect(1119, 100, 500, 50), "Hazard Tag: " + taggingTool);

        
        //if (prefabs.Length > 0 && selectedPrefab >= 0 && selectedPrefab < prefabs.Length)
        //{
        //    string prefabName = prefabs[selectedPrefab].name;
        //    GUI.color = new Color(1, 1, 1, 1);
        //    GUI.Label(new Rect(1110, 100, 200, 25), prefabName);
        //}
    }

    public void TagMovedObject()
    {
        selectedPrefab = 0;
        Debug.Log("Tagging Moved Object!");
        taggingTool = "Moved Object";
        spawnerActive = true;
    }

    public void TagRadiation()
    {
        selectedPrefab = 1;
        Debug.Log("Tagging Radiation!");
        taggingTool = "Radiation";
        spawnerActive = true;
    }

    public void TagFlammables()
    {
        selectedPrefab = 2;
        Debug.Log("Tagging Flammables!");
        taggingTool = "Flammable";
        spawnerActive = true;
    }

    public void TagCorrosion()
    {
        selectedPrefab = 3;
        Debug.Log("Tagging Corrosion!");
        taggingTool = "Corrosion/Caustic";
        spawnerActive = true;
    }

    public void TagCaution()
    {
        selectedPrefab = 4;
        Debug.Log("Tagging Other Caution!");
        taggingTool = "Caution";
        spawnerActive = true;
    }
}