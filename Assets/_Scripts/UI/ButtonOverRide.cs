using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* class ButtonOverRide
* inherits Button
* 
* this is a simple script used to override some of the button functions
* so that more info can be easily gathered on the button
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class ButtonOverRide : Button
{
    //true is the mouse is overing over the button
    public bool mouseHover;
    //true is the mouse is clicking on the button
    public bool mouseClicked;

    //if our controller is has selected it
    public bool cursorSelected;
    //if our controller has clicked it
    public bool cursorClicked;
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        mouseHover = true;
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        mouseHover = false;
        base.OnPointerExit(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        mouseClicked = true;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        mouseClicked = false;
        base.OnPointerUp(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }
}
