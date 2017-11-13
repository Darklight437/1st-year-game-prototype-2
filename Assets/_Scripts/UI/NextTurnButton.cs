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
    private Image Panel;
    void Start()
    {
        manager = GameManagment.FindObjectOfType<GameManagment>();

        ColorChangers = GameObject.FindGameObjectWithTag("RedVSBlue");
        //Put an if on whose turn to determine default UI color
        Panel = ColorChangers.GetComponent<Image>();

        if (manager.turn == 0)
        {
            Panel.color = Blue;
        }
        else
        {
            Panel.color = Red;
            
        }

    }

    void Update()
    {

        if (manager.turn == 0)
        {
            Panel.color = Blue;
        }
        else
        {
            Panel.color = Red;

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
