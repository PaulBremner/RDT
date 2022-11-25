using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class RadMeshCreator : MonoBehaviour
{

    //public string dataPath;
    //private string filename;

    public float scale = 1;
    public bool invertYZ = false;
    public bool forceReload = false;

    public int numPoints;
    public int numPointGroups;
    public int numMeshesLimit = 5;
    bool radVals = false;
    bool rgbVals = true;
    List<MeshData> meshData = new List<MeshData>();//create the list of meshes
    public float meshRadius = 1f;//size of the meshes
    private int limitPoints = 65000;
    public Material matVertex;
    //public string[] filePaths;
    //public int fileQuantity = 0;

    //public string outputFileName;
    //public Vector3 pcdPosDiff;
    //public bool CreateRadiationSources = false;

    //public Vector3 pointcloudPosLimits = new Vector3(7f, 10f, 3f);
    //public Vector3 pointcloudNegLimits = new Vector3(-14f, -9f, -1f);
    //public Collider boundingBox;

    //public Transform noisePoint;
    //Vector3 noisePos;

    private GameObject pointCloud;//container object for all the meshes
    List<GameObject> PcdFrames = new List<GameObject>();
    List<GameObject> GameGroup = new List<GameObject>();
    List<GameObject> RadGroup = new List<GameObject>();
    List<GameObject> RadObject = new List<GameObject>();

    public StreamWriter sw;

    /*
    public TextAsset radSources;
    public float radRadius = 1f;//cureemtly this results in a fixed rad strength for all sources, might need to make it so the rad radius also comes from the rad sources file
    Vector3[] radPoints;
    float[] radIntensities;
    */
    // GUI
    private float progress = 0;
    private string guiText;
    private bool loaded = false;

    public int density = 1;

    //public string filename_suffix = "";

    public class Co_ordinateFrame
    {
        Vector3 position;
        Quaternion rotation;

        public Co_ordinateFrame(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (matVertex == null)
        {
            Debug.LogError("assign the vertex material in the inspector before resuming");
        }

        //filename = new DirectoryInfo(@dataPath).Name + "Out";// outputFileName;//sets the file name to the directory name of the input directory + the 'Out' suffix// "multiFilepsd" + fileQuantity;
        /*filename_suffix = "_pcd_" + density.ToString();
        filename = Path.GetFileNameWithoutExtension(@dataPath) +  filename_suffix;// outputFileName

        if (fileQuantity == -1)
        {
            filePaths = new string[] { @dataPath };
            fileQuantity = 1;
        }
        else if (fileQuantity == 0)
        {
            fileQuantity = filePaths.Length;
        }
        else
        {
            filePaths = Directory.GetFiles(@dataPath, "*.ply");
        }

        noisePos = noisePoint.position;
        */
        //load file
        // Create Resources folder
        //CreateFolders();

        //boundingBox.gameObject.SetActive(true);// = true;
        //open the csv file
        //ignore the first line as it is the titles
        //store the data in the transforms from a list of GameObjects


        //if (CreateRadiationSources)
        //    ReadRadPoints();

        //print(File.Exists(Application.dataPath + dataPath));

        //print(meshData.Count);
        //LoadScene();
        /*sw = new StreamWriter("Assets/Resources/PointCloudMeshes/" + filename + @"/" + filename + "MeshList.txt");
        foreach (MeshData md in meshData)
        {
            if (md.Location.Count > 1)
                sw.WriteLine(md.GetNamePoint());
        }
        sw.Close();
        //instatiate each mesh in the list and save as a prefab

        boundingBox.gameObject.SetActive(false);
        */
    }
    /*
    private void ReadRadPoints()
    {
        string[] buffer = radSources.text.Split('\n');//store the rad points as a array of lines to be split later for comparing
        string[] buff = buffer[0].Split();

        radPoints = new Vector3[buffer.Length - 1];
        radIntensities = new float[buffer.Length - 1];
        for (int radIdx = 0; radIdx < buffer.Length - 1; radIdx++)
        {
            string[] radBuffer = buffer[radIdx].Split();
            radPoints[radIdx] = new Vector3(float.Parse(radBuffer[0]), float.Parse(radBuffer[1]), float.Parse(radBuffer[2]));
            radIntensities[radIdx] = float.Parse(radBuffer[3]) / 2;
        }
    }
    */




    /*
// read the points from the OFF file and create the meshes
void LoadOFF(string dPath)
{



    for (int fileidx = 0; fileidx < fileQuantity; fileidx++)
    {
        string filePath = filePaths[fileidx];
        // Read file
        BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open));

        ParseHeaderBinaryPLY(br);


        //Debug.Log(numPoints);
        Debug.Log(meshData.Count);
        //return;
        //numPoints = 3;//debug
        int meshDataPoints = 0;
        float noisedist = 100;
        Color noisecolor = new Color(146f/255f,137f/255f,132f/255f);
        Color noisecolor2 = new Color(145f / 255f, 137f / 255f, 134f / 255f);

        for (int pointIdx = 0; pointIdx < numPoints - 1; pointIdx++)//for each line in the file
        {
            Vector3 loc;
            Color rgb = Color.white;
            Color rad = Color.blue;
            try
            {
                float x = br.ReadSingle();
                float y = br.ReadSingle();
                float z = br.ReadSingle();
                uint red = br.ReadByte();
                uint green = br.ReadByte();
                uint blue = br.ReadByte();
                Vector3 normals = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                float curvature = br.ReadSingle();

                if (!invertYZ)
                    loc = new Vector3(x, y, z);
                else
                    loc = new Vector3(x, z, y);

                //Debug.Log("location " + pointIdx + " " + loc);
                if (rgbVals)//if there are RGBA values store them as a colour
                    rgb = new Color(red / 255.0f, green / 255.0f, blue / 255.0f);
                else
                    rgb = Color.white;
            }
            catch (EndOfStreamException)
            {
                Debug.Log("EOS Exeption");
                break;
            }

            if (CreateRadiationSources)
            {
                //get rad value for each rad point and store the highest in rad using HSVtoRGB to set the color values
                float radVal = 0.65f;
                for (int radIdx = 0; radIdx < radPoints.Length; radIdx++)
                {
                    Vector3 radPoint = radPoints[radIdx];
                    float radIntensity = radIntensities[radIdx];
                    float radValDist = Vector3.Distance(radPoint, loc);
                    if (radValDist < radRadius)
                    {
                        float radValPos = (radValDist / radIntensity) * 0.65f;
                        if (radValPos < radVal)
                            radVal = radValPos;
                    }
                }
                rad = Color.HSVToRGB(radVal, 1f, 1f);

            }

            if (meshDataPoints == limitPoints)
            {
                meshDataPoints = 0;
            }


            //if (loc.x > pointcloudNegLimits.x && loc.x < pointcloudPosLimits.x && loc.z > pointcloudNegLimits.z && loc.z < pointcloudPosLimits.z && loc.y > pointcloudNegLimits.y && loc.y < pointcloudPosLimits.y)
            float rboundlow = 145f / 255f;
            float rboundhigh = 147f / 255f;
            float gboundlow = 137f / 255f;
            float gboundhigh = 138f / 255f;
            float bboundlow = 131f / 255f;
            float bboundhigh = 134f / 255f;

            float colorDiffVal = Mathf.Abs(rgb.r - noisecolor.r) + Mathf.Abs(rgb.g - noisecolor.g) + Mathf.Abs(rgb.b - noisecolor.b);
            if (pointIdx % density == 0)
            {
                if (boundingBox.ClosestPoint(loc) == loc && colorDiffVal > (5f / 255f))//rgb != noisecolor && rgb != noisecolor2)//.r >= rboundlow && rgb.r <= rboundhigh && rgb.g >= gboundlow && rgb.g <= gboundhigh && rgb.b >= bboundlow && rgb.b <= bboundhigh)
                {
                    if (meshDataPoints == 0)
                    {
                        meshData.Add(new MeshData(loc, rgb, rad, fileidx));//add a new mesh to the list
                        meshDataPoints++;

                    }
                    else
                    {
                        meshData[meshData.Count - 1].AddPoint(loc, rgb, rad, fileidx);
                        meshDataPoints++;
                    }

                    //float disttonoiset = Vector3.Distance(loc, noisePos);
                    if (loc.y < -0.01)
                    {
                        noisecolor = rgb;
                    }
                }
            }

            // Relocate Points near the origin
            //calculateMin(points[i]);


            // GUI



        }

        MeshRenderer mr = noisePoint.GetComponent<MeshRenderer>();
        mr.material.color = noisecolor;

    }



    //prune mesh list to remove virtual points from the grid set up, and delete mesh containers - not doing this for now as not using mesh grid


    // Instantiate Point Groups

    pointCloud = new GameObject(filename);

    InstantiateMeshes();

    //Store PointCloud
    UnityEditor.PrefabUtility.SaveAsPrefabAsset(pointCloud, "Assets/Resources/PointCloudMeshes/" + filename + ".prefab");


    loaded = true;
}
*/
    public void CreateRadMesh(List<float[]> radData)
    {

        int meshDataPoints = 0;

        for (int pointIdx = 0; pointIdx < radData.Count; pointIdx++)//for each line in the file
        {
            Vector3 loc;
            Color rgb = Color.white;
            Color rad = Color.blue;

            float x = radData[pointIdx][0];
            float y = radData[pointIdx][1];
            float z = radData[pointIdx][2];

            //Vector3 normals = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            //float curvature = br.ReadSingle();

            if (!invertYZ)
                loc = new Vector3(x, y, z);
            else
                loc = new Vector3(x, z, y);

            //TODO check the range for radiation values so it sets hue correctly. I think currently I assume it is in the range 0-1
            rgb = Color.HSVToRGB((1 - radData[pointIdx][3]) * 0.65f, 1f, 1f);




            if (meshDataPoints == limitPoints)
            {
                meshDataPoints = 0;
            }


            if (meshData.Count >= numMeshesLimit)
            {
                meshData.RemoveAt(0);
                Destroy(PcdFrames[0]);
                Destroy(GameGroup[0]);
                Destroy(RadGroup[0]);
                Destroy(RadObject[0]);
                PcdFrames.RemoveAt(0);
                GameGroup.RemoveAt(0);
                RadGroup.RemoveAt(0);
                RadObject.RemoveAt(0);
            }

            if (pointIdx % density == 0)
            {

                if (meshDataPoints == 0)
                {
                    meshData.Add(new MeshData(loc, rgb, rad, 0));//add a new mesh to the list
                    meshDataPoints++;

                }
                else
                {
                    meshData[meshData.Count - 1].AddPoint(loc, rgb, rad, 0);
                    meshDataPoints++;
                }

            }

            // Relocate Points near the origin
            //calculateMin(points[i]);


            // GUI



        }

        //MeshRenderer mr = noisePoint.GetComponent<MeshRenderer>();
        //mr.material.color = noisecolor;





        //prune mesh list to remove virtual points from the grid set up, and delete mesh containers - not doing this for now as not using mesh grid


        // Instantiate Point Groups

        RadObject.Add(pointCloud = new GameObject("Radiation"));

        InstantiateMeshes();

        //Store PointCloud
        //UnityEditor.PrefabUtility.SaveAsPrefabAsset(pointCloud, "Assets/Resources/PointCloudMeshes/" + filename + ".prefab");


        //loaded = true;
    }


    private void InstantiateMeshes()
    {
        GameObject radMeshes = new GameObject("Rad");
        radMeshes.transform.parent = pointCloud.transform;
        GameObject PcdFrameRad = new GameObject();

        PcdFrames.Add(PcdFrameRad);
        RadGroup.Add(radMeshes);
        /*
            GameObject[] radSources = new GameObject[radPoints.Length];

            for (int rs = 0; rs < radPoints.Length; rs++)
            {
                radSources[rs] = new GameObject("Rad Source " + rs);
                radSources[rs].transform.position = radPoints[rs];
                radSources[rs].transform.parent = pointCloud.transform;
            }
        */




        foreach (MeshData md in meshData)
        {
            if (md.Location.Count > 1)//A meshData element should contain points in order to be instantiated, as the meshlist is inst with virtual points for grid centres, some can remain empty if there are no points in that part of the grid (and should be ignored)
            {//instantiate separate meshes for the different sensor types (RGB and rad for now)
             //print(md.Location.Count);



                if (PcdFrameRad.name != "Rad " + md.parent_ID)
                {
                    PcdFrameRad = new GameObject("Rad " + md.parent_ID);
                    PcdFrameRad.transform.parent = radMeshes.transform;
                }
                GameGroup.Add(InstantiatateMesh(md, md.ColorRGB.ToArray(), PcdFrameRad));


                //TODO:Add more mesh creation if we add different sensor data, there are plenty of other changes to accomodate more sensor types
            }
        }
    }

    private GameObject InstantiatateMesh(MeshData md, Color[] colors, GameObject parent)
    {
        GameObject pointGroup = new GameObject(parent.name + md.GetName());
        pointGroup.AddComponent<MeshFilter>();
        pointGroup.AddComponent<MeshRenderer>();
        pointGroup.GetComponent<Renderer>().material = matVertex;

        pointGroup.GetComponent<MeshFilter>().mesh = CreateMesh(md.Location.ToArray(), colors);
        pointGroup.transform.parent = parent.transform;

        return pointGroup;

        // Store Mesh
        /*UnityEditor.AssetDatabase.CreateAsset(pointGroup.GetComponent<MeshFilter>().mesh, "Assets/Resources/PointCloudMeshes/" + filename + @"/" + parent.name + md.GetName() + ".asset");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();*/
    }

    Mesh CreateMesh(Vector3[] points, Color[] colorVals)
    {

        Mesh mesh = new Mesh { vertices = points, colors = colorVals };
        int[] indecies = new int[points.Length];
        for (int i = 0; i < points.Length; i++) indecies[i] = i;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);
        mesh.uv = new Vector2[points.Length];
        mesh.normals = new Vector3[points.Length];


        return mesh;
    }

}
