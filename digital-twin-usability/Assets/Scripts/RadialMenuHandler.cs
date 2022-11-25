using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RadialMenuHandler : MonoBehaviour
{
    public SteamVR_Action_Boolean showMenu;
    public SteamVR_Action_Boolean optionSelected;
    public SteamVR_Action_Vector2 getSelection;
    public Hand hand;
    Canvas canvas;
    Button[] buttons;
    //public List<Image> highlights;
    public int buttonIdx = -1;
    int highlightedButton = -1;

    // Start is called before the first frame update
    void Start()
    {
        showMenu.AddOnChangeListener(ShowMenu, hand.handType);
        getSelection.AddOnChangeListener(GetSelection, hand.handType);
        optionSelected.AddOnChangeListener(OptionSelected, hand.handType);
        canvas = GetComponent<Canvas>();
        buttons = GetComponentsInChildren<Button>();
        //highlights = new List<Image>();
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.black;
            //highlights.Add(button.transform.Find("highlight").GetComponent<Image>());
        }
    }

    private void OptionSelected(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (buttonIdx >= 0)
        {
            if(newState == false)
                buttons[buttonIdx].onClick.Invoke();
            else
            {
                buttons[buttonIdx].GetComponentInChildren<ToggleHighlight>().ToggleImage();
                if(highlightedButton >= 0)
                    buttons[highlightedButton].GetComponentInChildren<ToggleHighlight>().ToggleImage();
                highlightedButton = buttonIdx;
            }
        }
    }

    private void GetSelection(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if(axis.y < 0)
        {
            if(axis.x < 0) SelectButton(0);
            else SelectButton(4);
        }
        else
        {
            if (axis.x < -0.5) SelectButton(1);
            else if (axis.x > 0.5) SelectButton(3);
            else SelectButton(2);
        }
    }

    private void SelectButton(int selectedButton)
    {
        if (buttonIdx >= 0)
            buttons[buttonIdx].GetComponent<Image>().color = Color.black;
        buttonIdx = selectedButton;
        buttons[buttonIdx].GetComponent<Image>().color = Color.green;
    }

    private void ShowMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        canvas.enabled = newState;
        if(newState == false && buttonIdx >= 0) buttons[buttonIdx].GetComponent<Image>().color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
