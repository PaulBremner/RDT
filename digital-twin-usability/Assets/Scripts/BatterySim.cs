using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BatterySim : MonoBehaviour

{
    public float timeRemaining = 300; // five minutes of battery time
    public float timeTotal = 300;
    public bool timerIsRunning = false;
    public TMP_Text timeText;
    public Slider mSlider;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void DisplayTime(float batterylife)
    {
        batterylife = Mathf.RoundToInt((timeRemaining / timeTotal) * 100);
        timeText.text = "Battery: " + batterylife.ToString() + " %";
        mSlider.value = batterylife;
    }

}