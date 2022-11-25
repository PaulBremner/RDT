using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpotBehaviour : MonoBehaviour
{
    private TcpListener spotTelemetrySocket, spotCommandSocket;
    private Thread spotTelemetryThread, spotCommandThread;
    private TcpClient spotTelemetryClient, spotCommandClient;
    //public TextMeshProUGUI GenericMessages;
    string spotTelemetryMessage, spotCommandMessage;
    string s_runtime, s_battperc, s_powerstate, s_chargestatus;
    public int SpotTelemetryPort, SpotCommandPort;
    public TextMeshProUGUI /*t_runtime*,*/ t_battperc/*, t_powerstate, t_chargestatus*/, t_connectionstatus;
    public Button SpotMission;
    public RawImage batt100, batt75, batt50, batt25, batt0, tick, cross;

    // Start is called before the first frame update
    void Start()
    {
        //connect to Spot
        s_runtime = "NOT CONNECTED";
        s_battperc = "NOT CONNECTED";
        s_powerstate = "NOT CONNECTED";
        s_chargestatus = "NOT CONNECTED";
        spotTelemetryThread = new Thread(new ThreadStart(SpotListenForIncomingTelemetry));
        spotTelemetryThread.IsBackground = true;
        spotTelemetryThread.Start();
        spotCommandThread = new Thread(new ThreadStart(SpotListenForIncomingCommands));
        spotCommandThread.IsBackground = true;
        spotCommandThread.Start();
        SpotMission.onClick.AddListener(spotMission1);
    }

    void spotMission1()
    {
        SendSpotCmd("a");
    }

    // Update is called once per frame
    void Update()
    {
        if (spotTelemetryMessage != null) //&& clientMessage != txt.text)
        {
            //t_runtime.text = "Estimated runtime: " + s_runtime.Split(':')[1] + "s";
            t_battperc.text = "Battery Status: " + s_battperc.Split(':')[1] + "%";
            tick.gameObject.SetActive(true);
            cross.gameObject.SetActive(false);
            //t_powerstate.text = "Motor power state: " + s_powerstate.Split(':')[1];
            //t_chargestatus.text = "Battery charge status: " + s_chargestatus.Split(':')[1];
            float battperc = float.Parse(s_battperc.Split(':')[1]);
            if(battperc >= 75)
            {
                batt100.gameObject.SetActive(true);
                batt75.gameObject.SetActive(false);
                batt50.gameObject.SetActive(false);
                batt25.gameObject.SetActive(false);
                batt0.gameObject.SetActive(false);
            }
            if (battperc >= 50 && battperc < 75)
            {
                batt75.gameObject.SetActive(true);
                batt100.gameObject.SetActive(false);
                batt50.gameObject.SetActive(false);
                batt25.gameObject.SetActive(false);
                batt0.gameObject.SetActive(false);
            }
            if (battperc >= 25 && battperc < 50)
            {
                batt50.gameObject.SetActive(true);
                batt100.gameObject.SetActive(false);
                batt75.gameObject.SetActive(false);
                batt25.gameObject.SetActive(false);
                batt0.gameObject.SetActive(false);
            }
            if (battperc >= 0 && battperc < 25)
            {
                batt25.gameObject.SetActive(true);
                batt100.gameObject.SetActive(false);
                batt75.gameObject.SetActive(false);
                batt50.gameObject.SetActive(false);
                batt0.gameObject.SetActive(false);
            }
            if (battperc == 0)
            {
                batt0.gameObject.SetActive(true);
                batt100.gameObject.SetActive(false);
                batt75.gameObject.SetActive(false);
                batt50.gameObject.SetActive(false);
                batt25.gameObject.SetActive(false);
            }
        }
        else
        {
            tick.gameObject.SetActive(false);
            cross.gameObject.SetActive(true);
        }
    }

    void SendSpotCmd(string str)
    {
        if (spotCommandClient == null)
        {
            Debug.Log("Socket Connection Null");
            return;
        }
        try
        {
            //encode the string to bytes
            byte[] _str = Encoding.ASCII.GetBytes(str);
            //get the outgoing network stream to write to
            NetworkStream stream = spotCommandClient.GetStream();
            //write the message to the stream
            stream.Write(_str, 0, _str.Length);
            Debug.Log("Message Sent");
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
    }

    #region SpotTelemetry
    void SpotListenForIncomingTelemetry()
    {
        try
        {
            spotTelemetrySocket = new TcpListener(IPAddress.Any, SpotTelemetryPort);
            spotTelemetrySocket.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            spotTelemetrySocket.Start();
            Debug.Log("Spot Telemetry Server listening...");
            //Set aside 1KB for the message
            byte[] bytes = new byte[1024];
            while (true)
            {
                using (spotTelemetryClient = spotTelemetrySocket.AcceptTcpClient())
                {
                    using (NetworkStream stream = spotTelemetryClient.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            spotTelemetryMessage = Encoding.ASCII.GetString(incomingData);
                            //Debug.Log("Spot Message received" + spotTelemetryMessage);
                            string[] datamsgs = spotTelemetryMessage.Split('\n');
                            s_runtime = datamsgs[0].TrimStart(' ');
                            s_battperc = datamsgs[1].TrimStart(' ');
                            s_powerstate = datamsgs[2].TrimStart(' ');
                            s_chargestatus = datamsgs[3].TrimStart(' ');
                            /*data order: 
                            1. estimated run time, 
                            2. battery percentage, 
                            3. motor power state (line 1), 
                            4. charge status (line 39), 
                            -------------------------------
                            5. stow state of arm (line 677), 
                            6. foot fl contact (line 574)
                            7. foot fr contact (line 601)
                            8. foot hl contact (line 628)
                            9. foot hr contact (line 655)
                            10. body in vision linear vel x (line 414)
                            11. body in vision linear vel y (line 415)
                            12. body in vision linear vel z (line 416)
                            13. body in odometry linear vel x (line 426)
                            14. body in odometry linear vel y (line 427)
                            15. body in odometry linear vel z (line 428)
                            16. hand in vision linear vel x (line 672)
                            17. hand in vision linear vel y (line 673)
                            18. hand in vision linear vel z (line 674)
                            19. hand in odometry linear vel x (line 698)
                            20. hand in odometry linear vel y (line 699)
                            21. hand in odometry linear vel z (line 700)*/
                        }
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
    }
    #endregion

    #region SpotCommands
    void SpotListenForIncomingCommands()
    {
        try
        {
            spotCommandSocket = new TcpListener(IPAddress.Any, SpotCommandPort);
            spotCommandSocket.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            spotCommandSocket.Start();
            //Debug.Log("Spot Command Server listening...");
            //Set aside 1KB for the message
            byte[] bytes = new byte[1024];
            while (true)
            {
                using (spotCommandClient = spotCommandSocket.AcceptTcpClient())
                {
                    using (NetworkStream stream = spotCommandClient.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            spotCommandMessage = Encoding.ASCII.GetString(incomingData);
                            Debug.Log("Message received" + spotCommandMessage);
                        }
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        if (spotTelemetrySocket != null)
        {
            spotTelemetrySocket.Stop();
            spotTelemetryThread.Abort();
        }
        if (spotCommandSocket != null)
        {
            spotCommandSocket.Stop();
            spotCommandThread.Abort();
        }
    }
}
