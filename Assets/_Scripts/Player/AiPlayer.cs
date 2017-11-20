#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AiPlayer
* child class of BasePlayer
* 
* object for a computer controller player, executes automated actions
* following a stratergy implemented by the behaviour tree 
* 
* author: Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class AiPlayer : BasePlayer
{
    //automated reference to the game management component
    private GameManagment manager = null;

    //automated reference to the map
    private Map map = null;

    //automated reference to the other player
    private BasePlayer opponent = null;

    //fuzzy logic machine for making decisions and moves
    public FuzzyLogic logicMachine;

    //AI stats
    public float advanceImportance = 1.0f; //scalar for scoring the benefits of moving up the map
    public float fleeImportance = 1.0f; //scalar for scoring the benefits of fleeing a fight
    public float attackImportance = 1.0f; //scalar for scoring the benefits of attacking enemies
    public float healingImportance = 1.0f; //scalar for scoring the benefits of stepping on a healing tile
    public float terrainImportance = 1.0f; //scalar for scoring the benefits of stepping on a defensive tile
    public float groupImportance = 1.0f; //scalar for scoring the benefits of moving towards the average group position
    public float kingBias = 20.0f; //additional points for the flee mechanic and penalty for the attack mechanic for the king only

    // Use this for initialization
    new void Start()
    {
        isHuman = false;
        manager = Object.FindObjectOfType<GameManagment>();
        map = Object.FindObjectOfType<Map>();
        logicMachine.input = GetComponent<ManagerInput>() as BaseInput;

        //get the size of the players list
        int playerSize = manager.players.Count;

        //iterate through all of the players
        for (int i = 0; i < playerSize; i++)
        {
            //store in a temp value for readability
            BasePlayer p = manager.players[i];

            //check that the current player isn't this one
            if (p != this as BasePlayer)
            {
                opponent = p;
                break;
            }
        }

        FuzzyFunction advance = new FuzzyFunction(null, null);
        FuzzyFunction flee = new FuzzyFunction(null, null);
        FuzzyFunction attack = new FuzzyFunction(null, null);
        FuzzyFunction healing = new FuzzyFunction(null, null);
        FuzzyFunction terrain = new FuzzyFunction(null, null);
        FuzzyFunction group = new FuzzyFunction(EvalGroup, ExecGroup);

        //add all fuzzy functions to the fuzzy logic machine
        //logicMachine.functions.Add(advance);
        //logicMachine.functions.Add(flee);
        //logicMachine.functions.Add(attack);
        //logicMachine.functions.Add(healing);
        //logicMachine.functions.Add(terrain);
        logicMachine.functions.Add(group);
    }


    // Update is called once per frame
    new void Update()
    {
    }

    /*
    * UpdateTurn 
    * overrides BasePlayers' UpdateTurn()
    * 
    * called once per frame while the player is active
    * 
    * @returns void
    */
    public override void UpdateTurn()
    {


        //get the size of the units list
        int unitSize = units.Count;

        //iterate through all units, making decisions about what to do with each
        for (int i = 0; i < unitSize; i++)
        {
            //store in a temp value for readability
            Unit unit = units[i];

            //up-cast the base input
            ManagerInput minput = logicMachine.input as ManagerInput;

            //set the correct unit in the input
            minput.unit = unit;

            //this will invoke the most appropriate callback
            logicMachine.Execute();
        }

        manager.OnNextTurn();

    }


    /*
    * Attack 
    * 
    * tells the game manager to apply an attacking action 
    * given the unit to attack with and it's target
    * 
    * @param Unit target - the unit to attack with
    * @param Vector3 targetPosition - the position to attack
    * @returns void
    */
    public void Attack(Unit target, Vector3 targetPosition)
    {
        manager.selectedUnit = target;
        manager.startTile = map.GetTileAtPos(target.transform.position);
        manager.endTile = map.GetTileAtPos(targetPosition);

        //execute a movement
        manager.OnActionSelected(0);
    }


    /*
    * Move 
    * 
    * tells the game manager to move a unit given
    * the unit to move and the target
    * 
    * @param Unit target - the unit to move
    * @param Vector3 targetPosition - the position to move to
    * @returns void
    */
    public void Move(Unit target, Vector3 targetPosition)
    {
        manager.selectedUnit = target;
        manager.startTile = map.GetTileAtPos(target.transform.position);
        manager.endTile = map.GetTileAtPos(targetPosition);

        //execute a movement
        manager.OnActionSelected(1);
    }


    /*
    * Ability 
    * 
    * tells the game manager to make a unit use it's special ability
    * given the unit to command and the target of the ability
    * 
    * @param Unit target - the unit to attack with
    * @param Vector3 targetPosition - the position to attack
    * @returns void
    */
    public void Ability(Unit target, Vector3 targetPosition)
    {
        manager.selectedUnit = target;
        manager.startTile = map.GetTileAtPos(target.transform.position);
        manager.endTile = map.GetTileAtPos(targetPosition);

        //execute a movement
        manager.OnActionSelected(2);
    }


    /*
    * EvalGroup 
    * 
    * fuzzy function used to determine if the group seeking function should be used
    * 
    * @param BaseInput inp - the input of the fuzzy logic machine
    * @returns float - the score evaluated
    */
    public float EvalGroup(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //get the length of the units list
        int unitCount = units.Count;

        //track the sum of all positions
        Vector3 sum = Vector3.zero;

        //get average position of the group
        for (int i = 0; i < unitCount; i++)
        {
            sum += units[i].transform.position;
        }

        //average group position
        Vector3 average = sum / unitCount;

        //get the maximum difference that the average position can possibly have from the unit
        float maxDifference = Mathf.Sqrt(map.width * map.width + map.height * map.height);

        //relative vector from the unit to the average
        Vector3 relative = average - minp.unit.transform.position;

        //0 = at the centre of the group, 1 = maximum possible difference that can be achieved given the map size
        return (relative.magnitude / maxDifference) * groupImportance;
    }


    public void ExecGroup(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //don't do anything if the unit has run out of points
        if (minp.unit.movementPoints == 0)
        {
            return;
        }

        //get a list of walkable tiles around the unit
        List<Tiles> walkable = GetArea.GetAreaOfMoveable(map.GetTileAtPos(minp.unit.transform.position), minp.unit.movementPoints, map);

        //get the length of the units list
        int unitCount = units.Count;

        //track the sum of all positions
        Vector3 sum = Vector3.zero;

        //get average position of the group
        for (int i = 0; i < unitCount; i++)
        {
            sum += units[i].transform.position;
        }

        //average group position
        Vector3 average = sum / unitCount;

        //worst possible score to start
        float bestScore = float.MaxValue;

        //reference to the best tile that has been found
        Tiles bestTile = null;

        //get the size of the walkable list
        int walkSize = walkable.Count;

        //iterate through all walkable tiles
        for (int i = 0; i < walkSize; i++)
        {
            //store in a temp variable
            Tiles tile = walkable[i];
          
            //get the squared distance to the tile
            float sqrDistance = (tile.transform.position - average).sqrMagnitude;

            if (sqrDistance < bestScore)
            {
                //reassign the best score
                bestTile = tile;
                bestScore = sqrDistance;
            }
        }

        //move towards the best tile if one was found
        if (bestTile != null)
        {
            Move(minp.unit, bestTile.pos);
        }
    }


}

#endif