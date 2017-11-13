using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    //list of all of the units the player owns
    public List<Unit> units = new List<Unit>();

    //position of the king, this is where the camera will go when the turn switches
    public Vector3 kingPosition = Vector3.zero;

    //ID of the player
    public int playerID = 0;

    //flag indicating that human input is expected on the 
    public bool isHuman = true;

    // Use this for initialization
    protected void Start ()
    {
		
	}

    // Update is called once per frame
    protected void Update ()
    {
		
	}


    /*
    * LinkIDs 
    * 
    * sets the local IDs of every unit owned
    * by this player
    * 
    * @param int ID - the ID of the player
    * @returns void
    */
    public void LinkIDs(int ID)
    {
        playerID = ID;

        //get the size of the units array once
        int unitsCount = units.Count;

        //iterate through all of the units, setting their IDs
        for (int i = 0; i < unitsCount; i++)
        {
            units[i].playerID = ID;
        }
    }


    /*
    * CalculateKingPosition
    * 
    * searches for the king and then sets a local variable
    * containing it's position
    * 
    * @returns voide
    */
    public void CalculateKingPosition()
    {
        //get the size of the units array once
        int unitsCount = units.Count;

        //iterate through all of the units, looking for the king
        for (int i = 0; i < unitsCount; i++)
        {
            //get the reference once (performance and readability)
            Unit unit = units[i];

            //check if the unit is the king
            if (unit is King)
            {
                //set the king position variable of the player
                kingPosition = new Vector3(unit.transform.position.x, 0.0f, unit.transform.position.z);
                return;
            }
        }
    }


    /*
    * IsBusy 
    * 
    * checks if any unit are still executing commands
    * 
    * @returns bool - the result of the check (true if at least one unit is still executing commands)
    */
    public bool IsBusy()
    {
        //get the size of the units array once
        int unitsCount = units.Count;

        //iterate through the units array, checking for a busy unit
        for (int i = 0; i < unitsCount; i++)
        {
            if (units[i].IsBusy())
            {
                return true;
            }
        }

        return false;
    }


    /*
    * UpdateTurn 
    * virtual function
    * 
    * called once per frame while the player is active
    * 
    * @returns void
    */
    public virtual void UpdateTurn()
    {

    }

}
