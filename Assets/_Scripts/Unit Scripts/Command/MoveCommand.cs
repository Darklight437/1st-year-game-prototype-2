using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class MoveCommand
* child class of UnitCommand
* 
* executes a Unit's movement routine
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class MoveCommand : UnitCommand
{
    //reference to the map
    public Map map = null;

    //list of tiles to follow
    private List<Tiles> m_tilePath = null;

    private Vector3 m_prevPos = Vector3.zero;
    private Vector3 DefaultForward;

    /*
    * MoveCommand()
    * 
    * constructor, specifies the target tile and callback
    * 
    * @param Unit u - the unit that made this command
    * @param VoidFunc scb - the callback to use when finished
    * @param VoidFunc fcb - the callback to use when failed
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    */
    public MoveCommand(Unit u, VoidFunc scb, VoidFunc fcb, Tiles st, Tiles et) : base(u, scb, fcb, st, et)
    {
        if (unit.ArtLink != null)
        {
            unit.ArtLink.SetBool("IsWalking", true);
        }
        //find the map component
        map = GameObject.FindObjectOfType<Map>();
        DefaultForward = unit.transform.forward;
    }


    /*
    * Update
    * overrides UnitCommand's Update()
    * 
    * called once per frame while the command is active
    * 
    * @returns void
    */
    public override void Update()
    {
        if (m_tilePath == null)
        {
            //get the start and end of the path
            Tiles startingTile = map.GetTileAtPos(unit.transform.position);

            startingTile.unit = null;

            //get the tile path to follow
            m_tilePath = AStar.GetAStarPath(startingTile, endTile);

            //the path is clear, and the unit can move there
            if (m_tilePath.Count > 0 && m_tilePath.Count <= unit.movementPoints)
            {
                //subtract the path distance from the movement points
                unit.movementPoints -= m_tilePath.Count;

                startingTile.unit = null;
                endTile.unit = unit;

                                                                        //testing                                           //Stop walking Anim
                                                                                                                          if (unit.ArtLink != null)
                                                                                                                          {
                                                                                                                              unit.ArtLink.SetBool("IsWalking", false);
                                                                                                                          }
            }
            else
            {
                //Stop walking Anim
                if (unit.ArtLink != null)
                {
                    unit.ArtLink.SetBool("IsWalking", false);
                }

                //the path failed
                startingTile.unit = unit;
                failedCallback();
                return;
            }
        }

        //check if there is still a path to follow
        if (m_tilePath.Count > 0)
        {
            //get the next position to go to
            Tiles nextTile = m_tilePath[0];

            //the 3D target of the movement
            Vector3 target = new Vector3(nextTile.pos.x, 0.5f, nextTile.pos.z);

            Vector3 relative = target - unit.transform.position;

            if (relative.magnitude < unit.movementSpeed * Time.deltaTime)
            {
                //this is a healing tile, heal the unit
                if (nextTile.IsHealing)
                {
                    unit.Heal(GameManagment.stats.tileHealthGained);
                }
                //this is a trap tile, it could kill the unit
                if (nextTile.tileType == eTileType.DAMAGE)
                {
                    if (unit.ArtLink != null)
                    {
                        unit.ArtLink.SetTrigger("TakeDamage");
                    }
                    unit.Defend(GameManagment.stats.trapTileDamage);
                }

                unit.transform.position = target;
                m_tilePath.RemoveAt(0);
            }
            else
            {
                unit.transform.position += relative.normalized * unit.movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (endTile.tileType == eTileType.DEFENSE)
            {
                //defensive buff
                unit.armour = unit.baseArmour + 1;
            }
            else
            {
                //remove the defensive buff
                unit.armour = unit.baseArmour;
            }

            //set the unit moving

            successCallback();

            //Stop walking Anim
            if (unit.ArtLink != null)
            {
                unit.ArtLink.SetBool("IsWalking", false);
            }


            if (m_prevPos != unit.transform.position)
            {
                unit.ArtLink.transform.forward = unit.transform.position - m_prevPos;
            }
            else
            {
                unit.ArtLink.transform.forward = DefaultForward;
            }
            m_prevPos = unit.ArtLink.transform.position;

            return;
        }
    }
}
