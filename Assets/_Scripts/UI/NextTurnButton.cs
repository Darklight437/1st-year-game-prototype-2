using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NextTurnButton : MonoBehaviour
{
    //automated reference to the game manager
    private GameManagment manager = null;

    //David
    public Color Red;
    public Color Blue;
    private GameObject ColorChangers;
    private SpriteRender spriteRender;

    void Start()
    {
        manager = GameManagment.FindObjectOfType<GameManagment>();

        ColorChangers = GameObject.FindGameObjectWithTag("RedVSBlue");

        //Put an if on whose turn to determine default UI color
        spriteRender = ColorChangers.GetComponent<SpriteRender>();

        if (manager.turn == 0)
        {
            spriteRender.isBlueTeam = true;
        }
        else
        {
            spriteRender.isBlueTeam = false;
        }

    }

    void Update()
    {
        if (manager.turn == 0)
        {
            spriteRender.isBlueTeam = true;
        }
        else
        {
            spriteRender.isBlueTeam = false;
        }
    }

    public void Click()
    {
        if (!manager.activePlayer.isHuman)
        {
            return;
        }

        
        //toggle bool turn
        manager.OnNextTurn();   
    }
	
}
