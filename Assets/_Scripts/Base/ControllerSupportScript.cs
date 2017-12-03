using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class ControllerSupportScript
* inherits MonoBehaviour
* 
* this class add in controler support for the Xbox One controller into the game scene
* so that players can play with a controller if they so wished
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class ControllerSupportScript : MonoBehaviour
{
    //refrence to the UI Manager
    public UIManager UImanager;

    //refrence to the set up of the camera and camera controls
    public CameraMovement cam;

    //refrence to the gamemanager
    public GameManagment gameManagment;

    //refrence to the map
    public Map map;

    //our "Virtual" cursor that the controler is linked to
    public Vector3 cursorPos;
    
    //this is the limits for our controler cursor pos clamping in with in the game bounds
    public Rect limits;

    //tile our cursor is currently hovering over
    public Tiles hoverOverTile;

    //how fast we can move our cursor
    public float cursorMoveSpeed;

    //wait time for if there has been no inputs from a controller we disable all visual stuff for the cursor
    public float disableTimer;
    //time to wait before disabling controler cursor again if it was used
    public float timeToDisableWaitTime;

    //the prefab for the cursor high light we want to use to show where the controler cursor is
    public GameObject cursorPrefab;
    //refrence to the cursor high light actually in the game
    public GameObject cursorHighLight;

    //the current active player ID
    public int playerID;

    //used to determin if we should end current players turn
    public float endTurnTimer;
    public float endTurnWaitTimer;

    //currently selected button
    public ButtonOverRide selectedButton;
    //what menu button position we are in;
    public int selectedButtonMenuPos;

    public GameObject[] actionButtons;

    //refrence to all the menu buttons
    public InGameMenuSpriteHolder inGameMenuRefrence;

    //delay on controler inputs so it does not get spamed
    public float waitTimer;

    public bool menuPress;

    public void Start()
    {
        selectedButton = null;
        selectedButtonMenuPos = inGameMenuRefrence.menuButtons.Length - 1;

        menuPress = false;

        //Get our limits form the camera set up
        limits = cam.limits;

        //start off with controler disabled
        disableTimer = timeToDisableWaitTime;

        //set player ID to player 1
        playerID = 0;
        
        //creat then disable our cursor highlight
        cursorHighLight = Instantiate(cursorPrefab, new Vector3(0,0,0), Quaternion.identity);
        cursorHighLight.SetActive(false);
    }

    public void Update()
    {
        //increase timers
        disableTimer += Time.deltaTime;
        waitTimer += Time.deltaTime;

        if (menuPress == true && Input.GetAxis("AButton") == 0)
        {
            selectedButton.onClick.Invoke();
            UImanager.currUIState = UIManager.eUIState.BASE;
            UImanager.stateSwitch();

            selectedButton.cursorClicked = false;
            selectedButton.cursorSelected = false;

            selectedButtonMenuPos = inGameMenuRefrence.menuButtons.Length - 1;
            selectedButton = null;
            inGameMenuRefrence.cursorActive = false;
            menuPress = false;

        }

        //check if we have swpaed player turns and set cursor to new players king position
        if (playerID != gameManagment.activePlayer.playerID)
        {
            gameManagment.activePlayer.CalculateKingPosition();
            hoverOverTile = map.GetTileAtPos(gameManagment.activePlayer.kingPosition);
            cursorPos = hoverOverTile.pos;

            cursorHighLight.transform.position = hoverOverTile.pos;

            playerID = gameManagment.activePlayer.playerID;
        }

        //check if controller input has not been used and if so set it to ddefault position 
        //disable it and go into a visual stand by
        if (disableTimer > timeToDisableWaitTime)
        {
            gameManagment.activePlayer.CalculateKingPosition();
            hoverOverTile = map.GetTileAtPos(gameManagment.activePlayer.kingPosition);
            cursorPos = hoverOverTile.pos;

            cursorHighLight.transform.position = hoverOverTile.pos;
            cursorHighLight.SetActive(false);

            for (int i = 0; i < actionButtons.Length; i++)
            {
                actionButtons[i].SetActive(false);
            }
        }
        else
        {
            //make sure the cursor highlight is active if in use
            cursorHighLight.SetActive(true);
            for (int i = 0; i < actionButtons.Length; i++)
            {
                actionButtons[i].SetActive(true);
            }
        }

        //deteck if player has put in any controller inputs
        DetectButtonPress();
    }

    /*
    * MoveCursor
    * public void function
    * 
    * this funtion reads in values from the left joystick
    * and moves our cursor by tem
    * 
    * @returns nothing
    */
    public void MoveCursor()
    {
        //read in the values from the joy sticks and discard if it is just noise
        float LeftAxis = Input.GetAxis("LeftJoyStickHorizontal");
        if (LeftAxis < 0.1f && LeftAxis > -0.1f)
        {
            LeftAxis = 0;
        }

        float UpAxis = Input.GetAxis("LeftJoyStickVertical");
        if (UpAxis < 0.1f && UpAxis > -0.1f)
        {
            UpAxis = 0;
        }

        //check wich player is currently player and reverse the controlles if it is player two
        //so that they stil visually work the same wayleft is still left right is right ectera
        if (gameManagment.activePlayer.playerID == 0)
        {
            if (LeftAxis > 0)
            {
                cursorPos.x += cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }
            else if (LeftAxis < 0)
            {
                cursorPos.x -= cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }

            if (UpAxis > 0)
            {
                cursorPos.z -= cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }
            else if (UpAxis < 0)
            {
                cursorPos.z += cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }
        }
        else
        {
            if (LeftAxis > 0)
            {
                cursorPos.x -= cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }
            else if (LeftAxis < 0)
            {
                cursorPos.x += cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }

            if (UpAxis > 0)
            {
                cursorPos.z += cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }
            else if (UpAxis < 0)
            {
                cursorPos.z -= cursorMoveSpeed * Time.deltaTime;
                disableTimer = 0;
            }

        }
       

        //CLAMP
        if (cursorPos.x < limits.x)
        {
            cursorPos.x = limits.x;
        }

        if (cursorPos.x > limits.width)
        {
            cursorPos.x = limits.width;
        }

        if (cursorPos.z < limits.y)
        {
            cursorPos.z = limits.y;
        }

        if (cursorPos.z > limits.height)
        {
            cursorPos.z = limits.height;
        }

        //after moving the cursor check if we are on new tile
        GatherTileOn();
    }

    /*
    * GatherTileOn
    * public void function
    * 
    * this gets the tile at the position of our virtual curosr
    * 
    * @returns nothing
    */
    public void GatherTileOn()
    {
        //if we do have a tile and our cursor position is still over it exit out now
        if (hoverOverTile != null)
        {
            if (((int)(cursorPos.x + 0.5f)) == ((int)(hoverOverTile.pos.x + 0.5f)) &&
                ((int)(cursorPos.z + 0.5f)) == ((int)(hoverOverTile.pos.z + 0.5f)))
            {
                return;
            }
        }

        //get the new tiles at cursor pos
        Tiles tile = map.GetTileAtPos(cursorPos);

        //if no tile was returned there is a problem throw an error
        if (tile == null)
        {
            Debug.LogError("Cursor Must be in illigale position");
            return;
        }

        //if the tile is the tile we already have return now as well
        if (tile == hoverOverTile)
        {
            return;
        }

        //if it is a diffrent tile then move cursor highlight over it
        //set our tile to it and move camera to focuse on it
        if (tile != hoverOverTile)
        {
            if (cursorHighLight.activeSelf == true)
            {
                cursorHighLight.transform.position = tile.pos;
            }

            hoverOverTile = tile;

            cam.Goto(hoverOverTile.pos, cam.transform.eulerAngles, null);

            return;
        }
    }

    /*
    * DetectButtonPress
    * public void function
    * 
    * this goes and checks to see if we have pressed any buttons on our controller
    * if those button presses are currently valid to use and activates there functions
    * as is appropriate
    * 
    * @returns nothing
    */
    public void DetectButtonPress()
    {
        //only run these checks when we do not have the ingame menu open
        if (UImanager.currUIState != UIManager.eUIState.PAUSEMENU)
        {
            //function to move the cursor
            MoveCursor();
            //checks for button presses when in game and playing
            InGameButtonChecks();
        }
        else
        {
            //checks for button presses when in the menu scene in game
            InGameMenuButtonCheck();
        }
        
    }

    public void InGameMenuButtonCheck()
    {
        //pressing the otions button on the controler opens up the menu
        if (Input.GetAxis("MenuButton") == 1 && waitTimer >= 0.5f)
        {
            UImanager.currUIState = UIManager.eUIState.BASE;
            UImanager.stateSwitch();
            disableTimer = 0;
            waitTimer = 0;

            selectedButtonMenuPos = inGameMenuRefrence.menuButtons.Length - 1;
            selectedButton = null;
            inGameMenuRefrence.cursorActive = false;
        }
        
        //scrolls "UP" the menu
        if (Input.GetAxis("LeftJoyStickVertical") < -0.1f && waitTimer >= 0.25f)
        {
            selectedButtonMenuPos -= 1;
            disableTimer = 0;
            waitTimer = 0;

            if (selectedButtonMenuPos < 0)
            {
                selectedButtonMenuPos = inGameMenuRefrence.menuButtons.Length - 1;
            }

            if (selectedButtonMenuPos > 0 || selectedButtonMenuPos < inGameMenuRefrence.menuButtons.Length)
            {
                selectedButton.cursorSelected = false;
                selectedButton.cursorClicked = false;
                selectedButton = inGameMenuRefrence.menuButtons[selectedButtonMenuPos];
                selectedButton.cursorSelected = true;
            }
            else
            {
                selectedButtonMenuPos = inGameMenuRefrence.menuButtons.Length - 1;
            }
        }

        //scrolls "down" the menu
        if (Input.GetAxis("LeftJoyStickVertical") > 0.1f && waitTimer >= 0.25f)
        {
            selectedButtonMenuPos += 1;
            disableTimer = 0;
            waitTimer = 0;

            if (selectedButtonMenuPos > inGameMenuRefrence.menuButtons.Length - 1)
            {
                selectedButtonMenuPos = 0;
            }

            if (selectedButtonMenuPos > 0 || selectedButtonMenuPos < inGameMenuRefrence.menuButtons.Length)
            {
                selectedButton.cursorSelected = false;
                selectedButton.cursorClicked = false;
                selectedButton = inGameMenuRefrence.menuButtons[selectedButtonMenuPos];
                selectedButton.cursorSelected = true;
            }
            else
            {
                selectedButtonMenuPos = 0;
            }
        }

        if (Input.GetAxis("AButton") == 1)
        {
            selectedButton.cursorClicked = true;
            menuPress = true;
        }
    }

    /*
    * InGameButtonChecks
    * public void function
    * 
    * this goes and checks to see if we have pressed any buttons on our controller
    * if those button presses are currently valid to use and activates there functions
    * as is appropriate while we are playing the game
    * 
    * @returns nothing
    */
    public void InGameButtonChecks()
    {
        //left click function call
        if (Input.GetAxis("LeftTrigger") == 1 && waitTimer > 0.5f)
        {
            gameManagment.OnTileSelectedLeftClick(hoverOverTile);
        }

        //right click function call
        if (Input.GetAxis("RightTrigger") == 1 && waitTimer > 0.5f)
        {
            Vector3 vector3 = Camera.main.WorldToScreenPoint(hoverOverTile.pos);
            gameManagment.OnTileSelectedRightClick(hoverOverTile, vector3);
        }

        //if player is holding down both trigger for more then the endTurnWaitTimer end current players turn
        if (Input.GetAxis("LeftTrigger") == 1 && Input.GetAxis("RightTrigger") == 1)
        {
            endTurnTimer += Time.deltaTime;
            if (endTurnTimer >= endTurnWaitTimer)
            {
                gameManagment.OnNextTurn();
                waitTimer = 0;
            }
        }
        else
        {
            endTurnTimer = 0;
        }

        //if any unit action UI is not at 10000 then it is valid to use
        if (UImanager.Buttons[0].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            //if we press the A button sends a move comand
            if (Input.GetAxis("AButton") == 1)
            {
                Debug.Log("AButton Pressed");
                gameManagment.OnActionSelected(1);
                disableTimer = 0;
                return;
            }
        }

        if (UImanager.Buttons[1].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            //if we press the X button sends a Attack comand
            if (Input.GetAxis("XButton") == 1)
            {
                Debug.Log("XButton Pressed");
                gameManagment.OnActionSelected(0);
                disableTimer = 0;
                return;
            }
        }

        if (UImanager.Buttons[2].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            //if we press the Y button sends a tile modife comand
            if (Input.GetAxis("YButton") == 1)
            {
                Debug.Log("YButton Pressed");
                gameManagment.OnActionSelected(2);
                disableTimer = 0;
                return;
            }
        }

        if (UImanager.Buttons[3].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            //if we press the B button sends a Cancel action comand
            if (Input.GetAxis("BButton") == 1)
            {
                Debug.Log("BButton Pressed");
                gameManagment.OnActionSelected(-1);
                disableTimer = 0;
                return;
            }
        }

        //pressing the otions button on the controler opens up the menu
        if (Input.GetAxis("MenuButton") == 1 && waitTimer > 0.5f)
        {
            UImanager.currUIState = UIManager.eUIState.PAUSEMENU;
            UImanager.stateSwitch();
            disableTimer = 0;
            waitTimer = 0;

            selectedButton = inGameMenuRefrence.menuButtons[selectedButtonMenuPos];
            selectedButton.cursorSelected = true;
            inGameMenuRefrence.cursorActive = true;
        }

        //left bumper toggles through the units in a "backwards" manner
        if (Input.GetAxis("LeftBumper") == 1 && waitTimer > 0.5f)
        {
            gameManagment.scroll = -1;
            gameManagment.ToggleBetweenActiveUnits();
            disableTimer = 0;
            waitTimer = 0;

            hoverOverTile = map.GetTileAtPos(gameManagment.selectedUnit.transform.position);
            cursorPos = gameManagment.selectedUnit.transform.position;
            cursorHighLight.transform.position = hoverOverTile.pos;

            return;
        }

        //left bumper toggles through the units in a "forwards" manner
        if (Input.GetAxis("RightBumper") == 1 && waitTimer > 0.5f)
        {
            gameManagment.scroll = 1;
            gameManagment.ToggleBetweenActiveUnits();
            disableTimer = 0;
            waitTimer = 0;

            hoverOverTile = map.GetTileAtPos(gameManagment.selectedUnit.transform.position);
            cursorPos = gameManagment.selectedUnit.transform.position;
            cursorHighLight.transform.position = hoverOverTile.pos;

            return;
        }
    }
}
