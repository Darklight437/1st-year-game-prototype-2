using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* class InGameMenuSpriteHolder
* inherits MonoBehaviour
* 
* this swaps out the menu sprite with the appropriate sprite depending on what the mouse is hovering over
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class InGameMenuSpriteHolder : MonoBehaviour
{
    //list of all the diffrent sprite states the menu can be in
    public Sprite[] menuSprits;
    //refrence to the menu where we will be setting our sprites
    public Image menu;

    //list of all the diffrent buttons that can change our sprite state
    public ButtonOverRide[] menuButtons;

    //active if menu was opened with controler
    public bool cursorActive;

    private void Awake()
    {
        //set menu sprite to default
        menu.sprite = menuSprits[0];

        Debug.Log("we started");

        //set default value
        cursorActive = false;
    }

    void Start ()
    {
	}
	
	void Update ()
    {
        if (cursorActive)
        {
            //update our sprite state based on what our controller has selected
            UpdateSpriteCursorPos();
        }
        else
        {
            //check to update our sprite state based on mouse position
            UpdateSpriteMousePos();
        }
	}

    /*
    * UpdateSpriteMousePos
    * public void function
    * 
    * this goes through all our buttons and checks if we need to update our sprite state
    * based on what the mouse is doing
    * 
    * @returns nothing
    */
    public void UpdateSpriteMousePos()
    {
        if (menuButtons[0].mouseHover == true && menuButtons[0].mouseClicked != true)
        {
            menu.sprite = menuSprits[1];
        }
        else if (menuButtons[0].mouseClicked == true)
        {
            menu.sprite = menuSprits[2];
        }
        else if (menuButtons[1].mouseHover == true && menuButtons[1].mouseClicked != true)
        {
            menu.sprite = menuSprits[3];
        }
        else if (menuButtons[1].mouseClicked == true)
        {
            menu.sprite = menuSprits[4];
        }
        else if (menuButtons[2].mouseHover == true && menuButtons[2].mouseClicked != true)
        {
            menu.sprite = menuSprits[5];
        }
        else if (menuButtons[2].mouseClicked == true)
        {
            menu.sprite = menuSprits[6];
        }
        else if (menuButtons[3].mouseHover == true && menuButtons[3].mouseClicked != true)
        {
            menu.sprite = menuSprits[7];
        }
        else if (menuButtons[3].mouseClicked == true)
        {
            menu.sprite = menuSprits[8];
        }
        else
        {
            menu.sprite = menuSprits[0];
        }
    }

    /*
    * UpdateSpriteCursorPos
    * public void function
    * 
    * this goes through all our buttons and checks if we need to update our sprite state
    * based on what the Controller curosr is doing
    * 
    * @returns nothing
    */
    public void UpdateSpriteCursorPos()
    {
        if (menuButtons[0].cursorSelected == true && menuButtons[0].cursorClicked != true)
        {
            menu.sprite = menuSprits[1];
        }
        else if (menuButtons[0].cursorClicked == true)
        {
            menu.sprite = menuSprits[2];
        }
        else if (menuButtons[1].cursorSelected == true && menuButtons[1].cursorClicked != true)
        {
            menu.sprite = menuSprits[3];
        }
        else if (menuButtons[1].cursorClicked == true)
        {
            menu.sprite = menuSprits[4];
        }
        else if (menuButtons[2].cursorSelected == true && menuButtons[2].cursorClicked != true)
        {
            menu.sprite = menuSprits[5];
        }
        else if (menuButtons[2].cursorClicked == true)
        {
            menu.sprite = menuSprits[6];
        }
        else if (menuButtons[3].cursorSelected == true && menuButtons[3].cursorClicked != true)
        {
            menu.sprite = menuSprits[7];
        }
        else if (menuButtons[3].cursorClicked == true)
        {
            menu.sprite = menuSprits[8];
        }
        else
        {
            menu.sprite = menuSprits[0];
        }
    }
}
