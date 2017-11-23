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

    private int m_teamMember = 0;

    //timer for thinking
    private float m_thinkTimer = 0.0f;
    public float m_idleTimer = 0.0f;

    public float idleTime = 2.0f;
    public float initialThinkTime = 1.0f;
    public float thinkTime = 10.0f;

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

        FuzzyFunction advance = new FuzzyFunction(EvalAdvance, ExecAdvance);
        FuzzyFunction flee = new FuzzyFunction(null, null);
        FuzzyFunction attack = new FuzzyFunction(EvalAttack, ExecAttack);
        FuzzyFunction healing = new FuzzyFunction(null, null);
        FuzzyFunction terrain = new FuzzyFunction(null, null);
        FuzzyFunction group = new FuzzyFunction(EvalGroup, ExecGroup);

        //add all fuzzy functions to the fuzzy logic machine
        logicMachine.functions.Add(advance);
        //logicMachine.functions.Add(flee);
        logicMachine.functions.Add(attack);
        //logicMachine.functions.Add(healing);
        //logicMachine.functions.Add(terrain);
        logicMachine.functions.Add(group);
    }


    // Update is called once per frame
    new void Update()
    {
    }

    /*
    * Reset
    * 
    * resets the thinking timers and flags for the AI
    * 
    * @returns void
    */
    public void Reset()
    {
        m_idleTimer = idleTime;
        m_thinkTimer = initialThinkTime;
        m_teamMember = 0;
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

        m_thinkTimer -= Time.deltaTime;

        //don't do anything if the think timer hasn't gone over the limit
        if (m_thinkTimer > 0.0f)
        {
            return;
        }

        //get the size of the units list
        int unitSize = units.Count;

        m_teamMember++;

        //reset the counter
        if (m_teamMember >= unitSize)
        {
            m_teamMember = 0;
        }

        //store in a temp value for readability
        Unit unit = units[m_teamMember];

        //up-cast the base input
        ManagerInput minput = logicMachine.input as ManagerInput;

        //set the correct unit in the input
        minput.unit = unit;

        //this will invoke the most appropriate callback
        logicMachine.Execute();

        if (IsBusy())
        {
            //reset the thinking timer
            m_thinkTimer = thinkTime;
        }
        else
        {
            //countdown the idle timer
            if (m_idleTimer > 0.0f)
            {
                m_idleTimer -= Time.deltaTime;
            }
            else
            {
                manager.OnNextTurn();
            }

        }

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
    * EvalAdvance 
    * 
    * fuzzy function used to determine if the group advancement function should be used
    * 
    * @param BaseInput inp - the input of the fuzzy logic machine
    * @returns float - the score evaluated
    */
    public float EvalAdvance(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //count the visible and total enemies of the opponents
        int visibleEnemies = 0;
        int totalEnemies = 0;

        //get the size of the players array
        int playerSize = minp.manager.players.Count;

        //iterate through all players except for this one, getting the amount of enemies that are visible and the total
        for (int i = 0; i < playerSize; i++)
        {
            //store in a temp variable
            BasePlayer bp = minp.manager.players[i];

            if (bp == this)
            {
                continue;
            }

            //get the size of the player's units array
            int unitCount = bp.units.Count;

            totalEnemies += unitCount;

            //check for visibility and count
            for (int j = 0; j < unitCount; j++)
            {
                if (bp.units[j].inSight)
                {
                    visibleEnemies++;
                }
            }
        }

        Debug.Log("Advance Score: " + ((1 - (float)visibleEnemies / (float)totalEnemies) * advanceImportance).ToString());

        return ((1 - (float)visibleEnemies / (float)totalEnemies)) * advanceImportance;
    }


    /*
    * ExecAdvance
    * 
    * fuzzy function for advancing the team to places that they havent explored
    * 
    * @param BaseInput inp - the object containing useful data that the function uses to determine what to do
    * @returns void
    */
    public void ExecAdvance(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //don't do anything if the unit has run out of points
        if (minp.unit.movementPoints == 0)
        {
            return;
        }

        //vector to the middle of the map
        Vector3 target = new Vector3(map.width, 0.0f, map.height) * 0.5f;

        ExecuteMovementGivenTarget(minp.unit, target);
    }


    /*
    * EvalAttack 
    * 
    * fuzzy function used to determine if the attack function should be used
    * 
    * @param BaseInput inp - the input of the fuzzy logic machine
    * @returns float - the score evaluated
    */
    public float EvalAttack(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //count the visible and total enemies of the opponents
        int visibleEnemies = 0;
        int totalEnemies = 0;

        //get the size of the players array
        int playerSize = minp.manager.players.Count;

        //iterate through all players except for this one, getting the amount of enemies that are visible and the total
        for (int i = 0; i < playerSize; i++)
        {
            //store in a temp variable
            BasePlayer bp = minp.manager.players[i];

            if (bp == this)
            {
                continue;
            }

            //get the size of the player's units array
            int unitCount = bp.units.Count;

            totalEnemies += unitCount;

            //check for visibility and count
            for (int j = 0; j < unitCount; j++)
            {
                if (bp.units[j].inSight)
                {
                    visibleEnemies++;
                }
            }
        }

        Debug.Log("Attack Score: " + (((float)visibleEnemies / (float)totalEnemies) * advanceImportance).ToString());

        return ((float)visibleEnemies / (float)totalEnemies) * advanceImportance;
    }


    /*
    * ExecAttack
    * 
    * fuzzy function for attacking the enemy with the (most health x potential DPS)
    * 
    * @param BaseInput inp - the object containing useful data that the function uses to determine what to do
    * @returns void
    */
    public void ExecAttack(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //don't do anything if the unit cannot attack
        if (minp.unit.hasAttacked)
        {
            return;
        }

        //get a list of walkable tiles around the unit
        List<Tiles> attackable = GetArea.GetAreaOfAttack(map.GetTileAtPos(minp.unit.transform.position), minp.unit.attackRange, map);

        //worst possible score to start
        float bestScore = -1.0f;

        //reference to the best tile that has been found
        Tiles bestTile = null;

        //get the size of the walkable list
        int attackSize = attackable.Count;

        //iterate through all walkable tiles
        for (int i = 0; i < attackSize; i++)
        {
            //store in a temp variable
            Tiles tile = attackable[i];

            //does the tile contain an enemy
            if (tile.unit != null)
            {
                if (tile.unit.playerID != playerID)
                {
                    //health ratio * damage per turn
                    float threatScore = (tile.unit.health / tile.unit.maxHealth) * tile.unit.damage;

                    if (bestScore < threatScore)
                    {
                        //reassign the best score
                        bestTile = tile;
                        bestScore = threatScore;
                    }
                }
            }
        }

        //attack the best tile if one is found
        if (bestTile != null)
        {
            Attack(minp.unit, bestTile.pos);
        }
        else
        {
            //get the worst threat and move towards it
            Unit bestTarget = null;
            float bestThreatScore = -1.0f;

            //get the size of the players list
            int playerSize = minp.manager.players.Count;

            //iterate through all players except for this one, getting the average of all enemies
            for (int i = 0; i < playerSize; i++)
            {
                //store in a temp variable
                BasePlayer bp = minp.manager.players[i];

                if (bp == this)
                {
                    continue;
                }

                //get the size of the player's units array
                int unitCount = bp.units.Count;

                //check for visibility and count
                for (int j = 0; j < unitCount; j++)
                {
                    if (bp.units[j].inSight)
                    {
                        //store in a temp variable
                        Unit unit = bp.units[j];

                        //the unit is in sight, consider it a threat
                        float threatScore = (unit.health / unit.maxHealth) * unit.damage;

                        //reset the score
                        if (bestThreatScore < threatScore)
                        {
                            bestTarget = unit;
                            bestThreatScore = threatScore;
                        }
                    }
                }
            }

            if (bestTarget != null)
            {
                //don't do anything if the unit has run out of points
                if (minp.unit.movementPoints == 0)
                {
                    return;
                }

                ExecuteMovementGivenTarget(minp.unit, bestTarget.transform.position);
            }
        }
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
        float maxDifference = Mathf.Sqrt(map.mapTiles.Count);

        //relative vector from the unit to the average
        Vector3 relative = average - minp.unit.transform.position;

        Debug.Log("Group Score: " + ((relative.magnitude / maxDifference) * groupImportance).ToString());

        //0 = at the centre of the group, 1 = maximum possible difference that can be achieved given the map size
        return (relative.magnitude / maxDifference) * groupImportance;
    }


    /*
    * ExecGroup
    * 
    * fuzzy function for regrouping the team
    * 
    * @param BaseInput inp - the object containing useful data that the function uses to determine what to do
    * @returns void
    */
    public void ExecGroup(BaseInput inp)
    {
        //up-cast the base input
        ManagerInput minp = inp as ManagerInput;

        //don't do anything if the unit has run out of points
        if (minp.unit.movementPoints == 0)
        {
            return;
        }

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

        ExecuteMovementGivenTarget(minp.unit, average);
    }


    /*
    * ExecuteMovementGivenTarget
    * 
    * given a target to move towards, find the 
    * closest tile and move that way
    * 
    * @param Unit unit - the unit that is undergoing the movement
    * @param Vector3 target - the vector to optimistically move towards
    * @returns void
    */
    public void ExecuteMovementGivenTarget(Unit unit, Vector3 target)
    {
        //get a list of walkable tiles around the unit
        List<Tiles> walkable = GetArea.GetAreaOfMoveable(map.GetTileAtPos(unit.transform.position), unit.movementPoints, map, unit);

        //worst possible score to start
        float bestMoveScore = float.MaxValue;

        //reference to the best tile that has been found
        Tiles bestTargetTile = null;

        //get the size of the walkable list
        int walkSize = walkable.Count;

        //iterate through all walkable tiles
        for (int i = 0; i < walkSize; i++)
        {
            //store in a temp variable
            Tiles tile = walkable[i];

            //get the squared distance to the tile
            float sqrDistance = (tile.transform.position - target).sqrMagnitude;

            if (sqrDistance < bestMoveScore && tile.unit == null)
            {
                //bestTargetTile the best score
                bestTargetTile = tile;
                bestMoveScore = sqrDistance;
            }
        }

        //move towards the best tile if one was found
        if (bestTargetTile != null)
        {
            Move(unit, bestTargetTile.pos);
        }
    }

}

#endif