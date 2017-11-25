using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class MenuMaster
* 
* container for all of the menus, remembers a stack of the currently accessed menus
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class MenuMaster : MonoBehaviour
{
    //reference to the menu that is currently being displayed
    private Menu activeMenu = null;

    //automated reference list of menus
    private List<Menu> menus = new List<Menu>();


    // Use this for initialization
    void Start()
    {
        //automatically get all of the menus in the scene
        menus = new List<Menu>(Resources.FindObjectsOfTypeAll<Menu>());

        //get the size of the menus list
        int menuCount = menus.Count;

        //iterate through all of the menus, initialising each
        for (int i = 0; i < menuCount; i++)
        {
            //store in a temp value
            Menu menu = menus[i];

            menu.Initialise();
            menu.gameObject.SetActive(false);
        }

        activeMenu = menus[0];
        activeMenu.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        //get the size of the menus list
        int menuCount = menus.Count;

        //iterate through all of the menus, updating each
        for (int i = 0; i < menuCount; i++)
        {
            //store in a temp value
            Menu menu = menus[i];

            menu.Loop();
        }
    }

    /*
    * ActivateMenu
    * 
    * calls the entry menu script and exit for all other menus
    * 
    * @param string id - the name of the menu to activate
    * @returns void
    */
    public void ActivateMenu(string id)
    {
        //get the size of the menus list
        int menuCount = menus.Count;

        //exit the active menu
        activeMenu.state = BaseAnimation.EdgeType.EXIT;
        activeMenu.Reset();

        //iterate through all of the menus, searching for the correct id string
        for (int i = 0; i < menuCount; i++)
        {
            //store in a temp value
            Menu menu = menus[i];

            //the menu has been found, activate it and stop searching
            if (menu.MENU_NAME == id)
            {
                menu.state = BaseAnimation.EdgeType.ENTRY;
                menu.Reset();
                
                activeMenu = menu;
                activeMenu.gameObject.SetActive(true);
                break;
            }
        }
    }
}
