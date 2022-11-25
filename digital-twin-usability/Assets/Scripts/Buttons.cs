using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buttons : MonoBehaviour
{
    public Button b_jackal, b_panda, b_scout, b_spot, b_telloedu, b_tello, b_scoutmini;
    private Button b_buttonclicked;
    public TextMeshProUGUI t_speed, t_latency, t_packet, t_battery, t_connection;
    private string s_roboInUse;

    // Start is called before the first frame update
    void Start()
    {
        b_jackal.onClick.AddListener(jackalClick);
        b_panda.onClick.AddListener(pandaClick);
        b_scout.onClick.AddListener(scoutClick);
        b_spot.onClick.AddListener(spotClick);
        b_telloedu.onClick.AddListener(telloeduClick);
        b_tello.onClick.AddListener(telloClick);
        b_scoutmini.onClick.AddListener(scoutminiClick);
    }

    // Update is called once per frame
    void Update()
    {
        //update text fields
        /*if roboInUse == "jackal" then
         * t_speed = jackal.speed()...
         * 
         * 
         * 
         * 
         * 
         */
    }

    void jackalClick()
    {
        b_jackal.interactable = false;
        b_panda.interactable = true;
        b_scout.interactable = true;
        b_spot.interactable = true;
        b_telloedu.interactable = true;
        b_tello.interactable = true;
        b_scoutmini.interactable = true;
        //set text fields to show info from jackal
    }

    void pandaClick()
    {
        b_panda.interactable = false;
        b_jackal.interactable = true;
        b_scout.interactable = true;
        b_spot.interactable = true;
        b_telloedu.interactable = true;
        b_tello.interactable = true;
        b_scoutmini.interactable = true;
    }

    void scoutClick()
    {
        b_scout.interactable = false;
        b_jackal.interactable = true;
        b_panda.interactable = true;
        b_spot.interactable = true;
        b_telloedu.interactable = true;
        b_tello.interactable = true;
        b_scoutmini.interactable = true;
    }

    void spotClick()
    {
        b_spot.interactable = false;
        b_jackal.interactable = true;
        b_panda.interactable = true;
        b_scout.interactable = true;
        b_telloedu.interactable = true;
        b_tello.interactable = true;
        b_scoutmini.interactable = true;
    }

    void telloeduClick()
    {
        b_telloedu.interactable = false;
        b_jackal.interactable = true;
        b_panda.interactable = true;
        b_scout.interactable = true;
        b_spot.interactable = true;
        b_tello.interactable = true;
        b_scoutmini.interactable = true;
    }

    void telloClick()
    {
        b_tello.interactable = false;
        b_jackal.interactable = true;
        b_panda.interactable = true;
        b_scout.interactable = true;
        b_spot.interactable = true;
        b_telloedu.interactable = true;
        b_scoutmini.interactable = true;
    }

    void scoutminiClick()
    {
        b_scoutmini.interactable = false;
        b_jackal.interactable = true;
        b_panda.interactable = true;
        b_scout.interactable = true;
        b_spot.interactable = true;
        b_telloedu.interactable = true;
        b_tello.interactable = true;
    }
}
