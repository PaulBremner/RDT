using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using RosMessageTypes.Tf2;
using Unity.Robotics.Visualizations;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
public class RadOccupancyToMesh : MonoBehaviour
{
    private ROSConnection ros;

    public string radDataOccupancyMapTopic = "/global_costmap_2d/costmap/costmap";

    public string radDataRawArrayTopic = "/global_costmap_2d/costmap/radiation/raw";
    public string tfFrame = "map";

    public int n_messagesToMesh = 1;
    private List<sbyte[]> occupancyGrid = new List<sbyte[]>();
    private List<float[]> radiationData = new List<float[]>();
    private uint occupancyGridWidth;
    private uint occupancyGridHeight;
    private float occupancyGridResolution;
    private Vector3 occupancyGridOriginPos;
    private Quaternion occupancyGridOriginOr;
    private Vector3 occupancyGridCalcPos;
    private Vector3 occupancyGridDrawOrigin;
    private List<float[]> radData = new List<float[]>();
    private RadMeshCreator radMeshCreator;
    static readonly int k_Color0 = Shader.PropertyToID("_Color0");
    static readonly int k_Color100 = Shader.PropertyToID("_Color100");
    static readonly int k_ColorUnknown = Shader.PropertyToID("_ColorUnknown");
    [SerializeField]
    Vector3 m_Offset = Vector3.zero;
    [SerializeField]
    Material m_Material;
    [SerializeField]
    private TFTrackingSettings m_TFTrackingSettings = new TFTrackingSettings { type = TFTrackingType.TrackLatest, tfTopic = "/tf" };
    [Header("Cell Colors (Dont Use)")]
    [SerializeField]
    Color m_Unoccupied = Color.white;
    [SerializeField]
    Color m_Occupied = Color.red;
    [SerializeField]
    Color m_Unknown = Color.cyan;
    // TEST from Unity-ROSHub
    Mesh m_Mesh;
    Texture2D m_Texture;
    bool m_TextureIsDirty = true;
    bool m_IsDrawingEnabled;
    public bool IsDrawingEnabled => m_IsDrawingEnabled;
    float m_LastDrawingFrameTime = -1;

    private Drawing3d m_Drawing;
    private OccupancyGridMsg m_Message;

    bool redrawGrid = true;
    void Start()
    {
        // Get ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        // Subscribe to topis and to process incomming data
        ros.Subscribe<OccupancyGridMsg>(radDataOccupancyMapTopic, ProcessOccupancyGrid);
        ros.Subscribe<Float32MultiArrayMsg>(radDataRawArrayTopic, ProcessRadRaw);
        // ros.Subscribe<TFMessageMsg>("/tf", ProcessTF);
    }

    public virtual void ProcessTF(TFMessageMsg message)
    {
        // Debug.Log("ProcessTF");
        for (int i = 0; i < message.transforms.Length; i++)
        {
            // Debug.Log(message.transforms[i].header.frame_id);
            if (message.transforms[i].header.frame_id.Equals(tfFrame))
            {
                // Debug.Log("Found!");
                // Debug.Log(message.transforms[i].transform.translation.x);
                // Debug.Log(message.transforms[i].transform.translation.y);
                // Debug.Log(message.transforms[i].transform.translation.z);
                occupancyGridCalcPos.x = occupancyGridOriginPos.x + (float)message.transforms[i].transform.translation.x;
                occupancyGridCalcPos.y = occupancyGridOriginPos.y + (float)message.transforms[i].transform.translation.y;
                occupancyGridCalcPos.z = occupancyGridOriginPos.z + (float)message.transforms[i].transform.translation.z;
            }
        }
    }
    public virtual void ProcessOccupancyGrid(OccupancyGridMsg message)
    {
        Debug.Log("ProcessOccupancyGrid");
        // Debug.Log(message.header);
        // Debug.Log(message.info);

        // sbyte[] r = new sbyte[message.data.Length];
        // r = (sbyte[])message.data.Clone();
        // occupancyGrid.Add(r);

        occupancyGridWidth = message.info.width;
        occupancyGridHeight = message.info.height;
        occupancyGridResolution = message.info.resolution;
        // Origin coordinates
        // occupancyGridOriginPos.x = (float)message.info.origin.position.x;
        // occupancyGridOriginPos.x = (float)message.info.origin.position.y;
        // occupancyGridOriginPos.z = (float)message.info.origin.position.z;

        occupancyGridOriginPos = message.info.origin.position.From<FLU>();
        occupancyGridOriginOr = message.info.origin.orientation.From<FLU>();
        occupancyGridOriginOr.eulerAngles += new Vector3(0, -90, 0);

        // Grid coordinates to use
        occupancyGridCalcPos.x = occupancyGridOriginPos.x;
        occupancyGridCalcPos.y = occupancyGridOriginPos.y;
        occupancyGridCalcPos.z = occupancyGridOriginPos.z;

        occupancyGridDrawOrigin = occupancyGridOriginPos - occupancyGridOriginOr * new Vector3(occupancyGridResolution * 0.5f, 0, occupancyGridResolution * 0.5f) + m_Offset;

        // Unity ROS-Hub
        m_Message = message;
        // Redraw();
    }
    public virtual void ProcessRadRaw(Float32MultiArrayMsg message)
    {
        Debug.Log("ProcessRaw");
        float[] r = new float[message.data.Length];
        r = (float[])message.data.Clone();
        if (radiationData.Count > 0)
            radiationData[0] = r;
        else
            radiationData.Add(r);
        if (radiationData.Count > 0 && occupancyGridWidth > 0 && radiationData.Count >= n_messagesToMesh && redrawGrid)
        {
            Debug.Log("Create mesh!");
            // UpdateTexture();
            StartCoroutine(OccupancyToMesh());
            // redrawGrid = false;
        }
        // Debug.Log(message.layout.data_offset);
        // Debug.Log(message.layout.dim[0].label);
        // Debug.Log(message.layout.dim[0].size);
        // Debug.Log(message.layout.dim[0].stride);
        // Debug.Log(message.data.Length);
        // Debug.Log(message.data[0]);
    }
    private IEnumerator OccupancyToMesh()
    {
        Debug.Log("Create mesh!");

        for (int width = 0; width < occupancyGridWidth; ++width)
        {
            for (int height = 0; height < occupancyGridHeight; ++height)
            {
                // if (m_Message.data[height * occupancyGridWidth + width] > 0)
                if (!float.IsNaN(radiationData[0][height * occupancyGridWidth + width]))
                {
                    float x = (width * occupancyGridResolution) + occupancyGridDrawOrigin.x;
                    float y = (height * occupancyGridResolution) + occupancyGridDrawOrigin.y;
                    float[] r = new float[] { x, 0.0f, y, Mathf.InverseLerp(0, 1000, radiationData[0][height * occupancyGridWidth + width]) };
                    radData.Add(r);
                }
            }
        }
        CreateMesh();
        yield return null;

    }

    public void Redraw()
    {
        if (m_Mesh == null)
        {
            m_Mesh = new Mesh();
            m_Mesh.vertices = new[]
            { Vector3.zero, new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0) };
            m_Mesh.uv = new[] { Vector2.zero, Vector2.up, Vector2.one, Vector2.right };
            m_Mesh.triangles = new[] { 0, 1, 2, 2, 3, 0 };
        }

        if (m_Material == null)
        {
            m_Material = new Material(Shader.Find("Unlit/OccupancyGrid"));
            // m_Material = new Material(Shader.Find("Unlit/ColorAlpha"));
        }
        m_Material.mainTexture = GetTexture();
        m_Material.SetColor(k_Color0, m_Unoccupied);
        m_Material.SetColor(k_Color100, m_Occupied);
        m_Material.SetColor(k_ColorUnknown, m_Unknown);

        var origin = m_Message.info.origin.position.From<FLU>();
        var rotation = m_Message.info.origin.orientation.From<FLU>();
        rotation.eulerAngles += new Vector3(0, -90, 0);
        var scale = m_Message.info.resolution;

        if (m_Drawing == null)
        {
            m_Drawing = Drawing3dManager.CreateDrawing();
        }
        else
        {
            m_Drawing.Clear();
        }

        m_Drawing.SetTFTrackingSettings(m_TFTrackingSettings, m_Message.header);
        // offset the mesh by half a grid square, because the message's position defines the CENTER of grid square 0,0
        Vector3 drawOrigin = origin - rotation * new Vector3(scale * 0.5f, 0, scale * 0.5f) + m_Offset;
        m_Drawing.DrawMesh(m_Mesh, drawOrigin, rotation,
            new Vector3(m_Message.info.width * scale, 1, m_Message.info.height * scale), m_Material);
    }

    public Texture2D GetTexture()
    {
        if (!m_TextureIsDirty)
            return m_Texture;

        if (m_Texture == null)
        {
            m_Texture = new Texture2D((int)m_Message.info.width, (int)m_Message.info.height, TextureFormat.R8, true);
            m_Texture.wrapMode = TextureWrapMode.Clamp;
            m_Texture.filterMode = FilterMode.Point;
        }
        else if (m_Message.info.width != m_Texture.width || m_Message.info.height != m_Texture.height)
        {
            m_Texture.Reinitialize((int)m_Message.info.width, (int)m_Message.info.height);
        }

        m_Texture.SetPixelData(m_Message.data, 0);
        m_Texture.Apply();
        m_TextureIsDirty = false;
        return m_Texture;
    }
    public void UpdateTexture()
    {
        for (int i = 0; i < radiationData[0].Length; i++)
        {
            if (float.IsNaN(radiationData[0][i]))
            {
                Debug.Log(i);
                radiationData[0][i] = 0.0f;
            }
        }
        Debug.Log("Update");
        m_Texture.SetPixelData(radiationData[0], 0);
        m_Texture.Apply();
        m_TextureIsDirty = false;
    }
    private void CreateMesh()
    {
        radMeshCreator = FindObjectOfType<RadMeshCreator>();//find an instance of the mesh creation script
        radMeshCreator.CreateRadMesh(radData);//call with list of radiation data, each call creates a new mesh with new data so don't call it too often
        radData.Clear();
    }
}
