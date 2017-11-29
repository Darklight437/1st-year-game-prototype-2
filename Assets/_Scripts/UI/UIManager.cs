using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //enum of states for the UI
    public enum eUIState { BASE, ENDGAME, PAUSEMENU, TURNCHANGE}
    public eUIState currUIState;

    public enum eCommandState { MSAC, MSC, ASC, MAC, MC, AC, SC, C, OFF }
    private eCommandState CurrentCommand = eCommandState.OFF;

    //all the UI elements in a play scene
    public GameObject PauseM = null;
    public GameObject TurnScr = null;
    
    
    //the spare rectTransforms that the buttons will sit at
    private Vector3[] ActivePos = new Vector3[5];
    private Vector3 InactivePos;
    //the core position that the buttons are childed to
    public GameObject MenuPosition;

    //the button Gameobjects
    public GameObject[] Buttons = new GameObject[4];
    private bool[] ButtonsStates = new bool[4];
    public Text DamageText = null;
    //should only be changed on & off at endgame
    public GameObject TurnButton = null;

    //the buttons on the winscreen
    public GameObject[] EndButtons = new GameObject[3];

    //Win screen for blue
    public GameObject BlueWin;
    //win screen for red
    public GameObject RedWin;
    //int of the winning player 0 blue, 1 red
    private int WinningPlayer = 2;
    //timer for Win screen buttons
    float timer = 0;

    // Use this for initialization
    void Start()
    {
        resetUI();
        //set the array of Vector 3s to the correct relative positions for the menu
        {
            
            ActivePos[0] = new Vector3(53.4f, -11.16f, 0);
            ActivePos[1] = new Vector3(53.4f, -36.7f, 0);
            ActivePos[2] = new Vector3(53.4f, -62.3f, 0);
            ActivePos[3] = new Vector3(53.4f, -88.3f, 0);
        }
        //set the inactive Vector 3s to the correct positions outside of any relative childing stuff
        {
            InactivePos = new Vector3(10000f, 10000f, 0f);
           

        }



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
            //turn on the buttons after the animation is finished
            if (timer == 0)
            {
                timer = 6;
            }
            if (timer != 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer < 0)
            {
                foreach (GameObject Button in EndButtons)
                {
                    Button.SetActive(true);
                }
            }

        }
    }

    //will check which UI elements should be active
    [HideInInspector]
   public void stateSwitch()
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



                if (WinningPlayer == 0)
                {
                    Instantiate(BlueWin);
                }
                else if (WinningPlayer == 1)
                {
                    Instantiate(RedWin);
                }
                TurnButton.SetActive(false);
                

                break;

            case eUIState.TURNCHANGE:
                resetUI();
                TurnScr.SetActive(true);
                break;


        }
    }
    [HideInInspector]
    public void GetWinningPlayer(int playerID)
    {
        WinningPlayer = playerID;
    }
    //turns off all of the ui elements currently active
    public void resetUI()
    {
        if (PauseM)
        {
            PauseM.SetActive(false);
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


    //interprets an enum to set each UI state for right click buttons
    public void ButtonState(eCommandState inComingCommand)
    {
        CurrentCommand = inComingCommand;
        switch (CurrentCommand)
        {

            case eCommandState.MSC:
                HideButtons();
                ButtonsStates[0] = true;
                ButtonsStates[2] = true;
                ButtonsStates[3] = true;
                moveButton();
                break;

            case eCommandState.ASC:
                HideButtons();
                ButtonsStates[1] = true;         
                ButtonsStates[2] = true;
                ButtonsStates[3] = true;
                moveButton();
                break;

            case eCommandState.MAC:
                HideButtons();
                ButtonsStates[0] = true;
                ButtonsStates[1] = true;
                ButtonsStates[3] = true;
                moveButton();
                break;

            case eCommandState.MC:
                HideButtons();
                ButtonsStates[0] = true;
                ButtonsStates[3] = true;
                moveButton();
                break;

            case eCommandState.AC:
                HideButtons();
                ButtonsStates[1] = true;
                ButtonsStates[3] = true;
                moveButton();
                break;

            case eCommandState.SC:
                HideButtons();
                ButtonsStates[2] = true;
                ButtonsStates[3] = true;
                moveButton();
                moveButton();
                break;

            case eCommandState.C:
                HideButtons();
                ButtonsStates[3] = true;
                moveButton();                
                break;

            case eCommandState.OFF:
                moveButton();
                HideButtons();
                break;
        }
    }

    private void HideButtons()
    {

        for (int i = 0; i < ButtonsStates.Length; i++)
        {
            ButtonsStates[i] = false;
        }
        moveButton();

    }
    private void moveButton()
    {
        int ButtonPositionIndex = 0;

        for (int i = 0; i < ButtonsStates.Length; i++)
        {
            if (ButtonsStates[i] == true)
            {
                Buttons[i].GetComponent<RectTransform>().localPosition = ActivePos[ButtonPositionIndex];
               

                ButtonPositionIndex++;
            }
            else
            {

                Buttons[i].GetComponent<RectTransform>().anchoredPosition = InactivePos;
                
                
            }
        }
    }
}
