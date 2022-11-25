using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PandaBehaviour : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread pandaTelemetryThread;
    string pandaMessage;
    public int PandaPort;
    private float j0, j1, j2, j3, j4, j5, j6;
    public Button organize, spothandover;

    // Start is called before the first frame update
    void Start()
    {
        pandaTelemetryThread = new Thread(new ThreadStart(PandaListen));
        pandaTelemetryThread.IsBackground = true;
        pandaTelemetryThread.Start();
        //connect to Panda
        organize.onClick.AddListener(pandaorganise);
        spothandover.onClick.AddListener(pandaspothandover);
    }

    // Update is called once per frame
    void Update()
    {
        //split message, 0 = joint 0, 1 = joint 1, 2 = joint 2 etc.
        //set joint positions
        //"nuclearorganize"
        //"spothandover"

        Debug.Log(j0 + j1 + j2);
    }

    void pandaorganise()
    {
        byte[] msg = Encoding.UTF8.GetBytes("nuclearorganize");
        udpClient.Send(msg, msg.Length);
    }

    void pandaspothandover()
    {
        byte[] msg = Encoding.UTF8.GetBytes("spothandover");
        udpClient.Send(msg, msg.Length);
    }

    void PandaListen()
    {
        try
        {
            udpClient = new UdpClient(1101);
            udpClient.Connect("10.0.0.15", 1101);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 1100);
            byte[] received = udpClient.Receive(ref iPEndPoint);
            pandaMessage = Encoding.UTF8.GetString(received);
            j0 = float.Parse(pandaMessage.Split(',')[0]);
            j1 = float.Parse(pandaMessage.Split(',')[1]);
            j2 = float.Parse(pandaMessage.Split(',')[2]);
            j3 = float.Parse(pandaMessage.Split(',')[3]);
            j4 = float.Parse(pandaMessage.Split(',')[4]);
            j5 = float.Parse(pandaMessage.Split(',')[5]);
            j6 = float.Parse(pandaMessage.Split(',')[6]);
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
    }
}
