using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    //enum of states for the UI
    public enum eUIState { BASE, ENDGAME, PAUSEMENU, TURNCHANGE}
    public eUIState currUIState;

    public enum eCommandState { MSAC, MSC, ASC, MAC, MC, AC, SC, C, OFF }
    private eCommandState CurrentCommand = eCommandState.OFF;

    //all the UI elements in a play scene
    public GameObject PauseM = null;
    public GameObject EndM = null;
    public GameObject TurnScr = null;
    

    //the spare rectTransforms that the buttons will sit at
    public RectTransform[] ButtonPos = new RectTransform[5];
    //the core position that the buttons are childed to
    public GameObject MenuPosition;

    //the button Gameobjects
    public GameObject[] Buttons = new GameObject[4];



    // Use this for initialization
    void Start()
    {
        resetUI();
        //make function that clears rect transforms of buttons

    }

    // Update is called once per frame
    void Update()
    {

        Canvas.ForceUpdateCanvases();
        //switch on enum
        //enable / disable elements of ui & manage UI anims

        //clear ui to base state
        if (currUIState == eUIState.BASE)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.PAUSEMENU;
                stateSwitch();
            }
        }
       else if (currUIState == eUIState.PAUSEMENU)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.BASE;
                stateSwitch();
            }
        }
       else if (currUIState == eUIState.ENDGAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currUIState = eUIState.BASE;
                stateSwitch();
            }
        }
    }

    //will check which UI elements should be active
    void stateSwitch()
    {
        switch (currUIState)
        {
            case eUIState.BASE:
                resetUI();
                break;

            case eUIState.PAUSEMENU:
                resetUI();
                PauseM.SetActive(true);
                break;

            case eUIState.ENDGAME:
                resetUI();
                EndM.SetActive(true);
                break;

            case eUIState.TURNCHANGE:
                resetUI();
                //TurnScr.setActive(true);
                break;


        }
    }
    //turns off all of the ui elements currently active
    public void resetUI()
    {
        if (PauseM)
        {
            PauseM.SetActive(false);
        }
        if (EndM)
        {
            EndM.SetActive(false);
        }
        if (TurnScr)
        {
            TurnScr.SetActive(false);
        }

        //currUIState = eUIState.BASE;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void ButtonState(eCommandState inComingCommand)
    {
        CurrentCommand = inComingCommand;
        switch (CurrentCommand)
        {

            case eCommandState.MSC:
                turnOffButtons();
                Buttons[0].SetActive(true);
                Buttons[2].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                break;

            case eCommandState.ASC:
                turnOffButtons();
                Buttons[1].SetActive(true);
                Buttons[2].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                break;

            case eCommandState.MAC:
                turnOffButtons();
                Buttons[0].SetActive(true);
                Buttons[1].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                break;

            case eCommandState.MC:
                turnOffButtons();
                Buttons[0].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                break;

            case eCommandState.AC:
                turnOffButtons();
                Buttons[1].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                break;

            case eCommandState.SC:
                turnOffButtons();
                Buttons[2].SetActive(true);
                Buttons[3].SetActive(true);
                moveButton();
                moveButton();
                break;

            case eCommandState.C:
                turnOffButtons();
                Buttons[3].SetActive(true);
                moveButton();                
                break;

            case eCommandState.OFF:
                moveButton();
                turnOffButtons();
                break;
        }
    }

    private void turnOffButtons()
    {
        //set buttons to a position off scren?
        Buttons[0].SetActive(false);
        Buttons[1].SetActive(false);
        Buttons[2].SetActive(false);
        Buttons[3].SetActive(false);

    }
    private void moveButton()
    {
        int ButtonPositionIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            if (Buttons[i].activeInHierarchy)
            {
                Buttons[i].GetComponent<RectTransform>().position = ButtonPos[ButtonPositionIndex].position;
                ButtonPositionIndex++;
            }
        }
    }
}
