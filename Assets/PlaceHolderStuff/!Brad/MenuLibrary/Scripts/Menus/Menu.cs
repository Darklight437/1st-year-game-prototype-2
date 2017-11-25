using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Menu
* 
* container for a set of related items, calls related update functions on items
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class Menu : MonoBehaviour
{

    //basically an ID for the menu-master to identify the menu with
    public string MENU_NAME = "";

    //automated reference list of items
    private List<Item> items = new List<Item>();

    //the current state of the menu
    public BaseAnimation.EdgeType state = BaseAnimation.EdgeType.NONE;

    // Use this for initialization
    public void Initialise()
    {
        //automatically get all of the items in the game-object
        items = new List<Item>(GetComponentsInChildren<Item>());

        //get the size of the items list
        int itemCount = items.Count;

        //iterate through the items, initialising each
        for (int i = 0; i < itemCount; i++)
        {
            //store in a temp value
            Item item = items[i];

            item.Initialise();
        }
    }


    /*
    * Reset 
    * 
    * resets all of the items
    * 
    * @returns void
    */
    public void Reset()
    {
        //get the size of the items list
        int itemCount = items.Count;  

        //iterate through the items, resetting each's timer
        for (int i = 0; i < itemCount; i++)
        {
            //store in a temp value
            Item item = items[i];

            int entryCount = item.entries.Count;
            int exitCount = item.exits.Count;

            //reset the base timers of the items
            for (int j = 0; j < entryCount; j++)
            {
                item.entries[j].baseTimer = 0.0f;
                item.entries[j].lerpValue = 1.0f;
            }

            for (int j = 0; j < exitCount; j++)
            {
                item.exits[j].baseTimer = 0.0f;
                item.entries[j].lerpValue = 1.0f;
            }
        }
    
    }


    /*
    * Loop 
    * 
    * called once per frame while active
    * 
    * @returns void
    */
    public void Loop()
    {
        //determine the state of the menu
        if (state == BaseAnimation.EdgeType.NONE)
        {
            //do nothing?
        }
        else if (state == BaseAnimation.EdgeType.ENTRY)
        {
            //get the size of the items list
            int itemCount = items.Count;

            //iterate through the items, updating each with the correct input state
            for (int i = 0; i < itemCount; i++)
            {
                //store in a temp value
                Item item = items[i];

                item.Loop(BaseAnimation.EdgeType.ENTRY);
            }
        }
        else if (state == BaseAnimation.EdgeType.EXIT)
        {
            //get the size of the items list
            int itemCount = items.Count;

            //iterate through the items, updating each with the correct input state
            for (int i = 0; i < itemCount; i++)
            {
                //store in a temp value
                Item item = items[i];

                item.Loop(BaseAnimation.EdgeType.EXIT);
            }
        }


        //this block runs accross entry and exit states
        if (state == BaseAnimation.EdgeType.ENTRY || state == BaseAnimation.EdgeType.EXIT)
        {
            //flag identifying if animations are still running
            bool isRunning = false;

            //get the size of the items list
            int itemCount = items.Count;

            //iterate through the items, updating each with the correct input state
            for (int i = 0; i < itemCount; i++)
            {
                //store in a temp value
                Item item = items[i];

                if (item.IsAnimating())
                {
                    isRunning = true;
                }
            }

            //turn off the menu if it is no longer doing anything
            if (!isRunning)
            {
                //deactivate a menu on an exit animation
                if (state == BaseAnimation.EdgeType.EXIT)
                {
                    gameObject.SetActive(false);
                }

                state = BaseAnimation.EdgeType.NONE;
            }
        }
    }
}
