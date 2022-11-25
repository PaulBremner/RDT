// Dr Erwin Lopez <erwin.lopezpulgarin@manchester.ac.uk>
// Changelog:
// 2022-08-18 13:41:01  Read /statistics topic and store data per robot

// Topic Statistic functionality
// http://wiki.ros.org/Topics#Topic_statistics
// TopicStatistics description
// http://docs.ros.org/en/api/rosgraph_msgs/html/msg/TopicStatistics.html
// Unity description
// https://github.com/Unity-Technologies/ROS-TCP-Connector/blob/main/com.unity.robotics.ros-tcp-connector/Runtime/Messages/Rosgraph/msg/TopicStatisticsMsg.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Rosgraph;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;

// NOTE: We need to run the following command from the ROS side in order to enable statistics:
// rosparam set enable_statistics true
public class MasterStatisticsReader : MonoBehaviour
{
    private ROSConnection ros;
    private string statisticsTopic = "/statistics";
    [Tooltip("Topics relecant to ROS Topic name for Arbitary text message")]
    public List<string> topicsRobot1;
    public List<string> topicsRobot2;
    public List<string> topicsRobot3;
    public List<string> topicsRobot4;
    public List<string> topicsRobot5;
    public List<string> topicsRobot6;
    public List<string> topicsRobot7;
    public TopicStatisticsMsg statisticsRobot1;
    public TopicStatisticsMsg statisticsRobot2;
    public TopicStatisticsMsg statisticsRobot3;
    public TopicStatisticsMsg statisticsRobot4;
    public TopicStatisticsMsg statisticsRobot5;
    public TopicStatisticsMsg statisticsRobot6;
    public TopicStatisticsMsg statisticsRobot7;

    private Float32MultiArrayMsg test1;
    private OccupancyGridMsg test2;
    void Start()
    {
        // Get ROS connection
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<TopicStatisticsMsg>(statisticsTopic, ReadStatistics);
    }

    // Current strategy is to have ONE topic per robot, getting its statistics and using it as relevant for the entire robot.
    // Relevant data is:
    // Delivered and dropped messages:
    //      delivered_msgs
    //      dropped_msgs
    // Traffic in bytes (divide by 1000 to have it in Kbytes)
    //      traffic
    // Timing between messages
    //      period_mean
    //      period_stddev
    //      period_max
    public virtual void ReadStatistics(TopicStatisticsMsg message)
    {
        // Next strategy can be to aggregate and average them
        if (topicsRobot1.Contains(message.topic))
        {
            Debug.Log("Robot 1");
            statisticsRobot1 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot2.Contains(message.topic))
        {
            Debug.Log("Robot 2");
            statisticsRobot2 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot3.Contains(message.topic))
        {
            Debug.Log("Robot 3");
            statisticsRobot3 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot4.Contains(message.topic))
        {
            Debug.Log("Robot 4");
            statisticsRobot4 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot5.Contains(message.topic))
        {
            Debug.Log("Robot 5");
            statisticsRobot5 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot6.Contains(message.topic))
        {
            Debug.Log("Robot 6");
            statisticsRobot6 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
        else if (topicsRobot7.Contains(message.topic))
        {
            Debug.Log("Robot 7");
            statisticsRobot7 = message;
            Debug.Log(message.topic);
            // in bytes
            Debug.Log(message.delivered_msgs);
            Debug.Log(message.dropped_msgs);
            Debug.Log(message.traffic / 1000);

            Debug.Log(message.period_mean);
            Debug.Log(message.period_stddev);
            Debug.Log(message.period_max);
        }
    }
}
