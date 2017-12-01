using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSupportScript : MonoBehaviour
{
    public UIManager UIManager;

    public CameraMovement cam;

    public GameManagment gameManagment;

    public Map map;

    public Vector3 cursorPos;
    
    public Rect limits;

    public Tiles hoverOverTile;

    public float cursorMoveSpeed;

    public float disableTimer;
    public float timeToDisableWaitTime;


    public GameObject cursorPrefab;
    public GameObject cursorHighLight;

    public void Start()
    {
        limits = cam.limits;

        disableTimer = timeToDisableWaitTime;
        
        cursorHighLight = Instantiate(cursorPrefab, new Vector3(0,0,0), Quaternion.identity);
        cursorHighLight.SetActive(false);
    }

    public void Update()
    {
        disableTimer += Time.deltaTime;

        if (disableTimer > timeToDisableWaitTime)
        {
            cursorHighLight.SetActive(false);
            hoverOverTile = map.GetTileAtPos(gameManagment.activePlayer.kingPosition);
            cursorPos = hoverOverTile.pos;
        }
        else
        {
            cursorHighLight.SetActive(true);
        }

        MoveCursor();
        
        if (Input.GetAxis("LeftTrigger") == 1)
        {
            gameManagment.OnTileSelectedLeftClick(hoverOverTile);
        }

        if (Input.GetAxis("RightTrigger")== 1)
        {
            Vector3 vector3 = Camera.main.WorldToScreenPoint(hoverOverTile.pos);
            gameManagment.OnTileSelectedRightClick(hoverOverTile, vector3);
        }

        DetectButtonPress();
    }

    public void MoveCursor()
    {

        float LeftAxis = Input.GetAxis("LeftJoyStickHorizontal");

        if (LeftAxis < 0.1f && LeftAxis > -0.1f)
        {
            LeftAxis = 0;
        }

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

        float UpAxis = Input.GetAxis("LeftJoyStickVertical");

        if (UpAxis < 0.1f && UpAxis > -0.1f)
        {
            UpAxis = 0;
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

        GatherTileOn();
    }

    public void GatherTileOn()
    {
        if (hoverOverTile != null)
        {
            if (((int)(cursorPos.x + 0.5f)) == ((int)(hoverOverTile.pos.x + 0.5f)) &&
                ((int)(cursorPos.z + 0.5f)) == ((int)(hoverOverTile.pos.z + 0.5f)))
            {
                return;
            }
        }

        Tiles tile = map.GetTileAtPos(cursorPos);

        if (tile == null)
        {
            Debug.LogError("Cursor Must be in illigale position");
            return;
        }

        if (tile == hoverOverTile)
        {
            return;
        }

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

    public void DetectButtonPress()
    {
        if (UIManager.Buttons[0].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            if (Input.GetAxis("AButton") == 1)
            {
                Debug.Log("AButton Pressed");
                gameManagment.OnActionSelected(1);
                disableTimer = 0;
                return;
            }
        }

        if (UIManager.Buttons[1].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            if (Input.GetAxis("XButton") == 1)
            {
                Debug.Log("XButton Pressed");
                gameManagment.OnActionSelected(0);
                disableTimer = 0;
                return;
            }
        }

        if (UIManager.Buttons[2].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            if (Input.GetAxis("YButton") == 1)
            {
                Debug.Log("YButton Pressed");
                gameManagment.OnActionSelected(2);
                disableTimer = 0;
                return;
            }
        }

        if (UIManager.Buttons[3].GetComponent<RectTransform>().anchoredPosition.x != 10000)
        {
            if (Input.GetAxis("BButton") == 1)
            {
                Debug.Log("BButton Pressed");
                gameManagment.OnActionSelected(-1);
                disableTimer = 0;
                return;
            }
        }
    }
}
