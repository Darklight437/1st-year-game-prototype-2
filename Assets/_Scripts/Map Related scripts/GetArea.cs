using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class GetArea
* 
* this finds all tiles around a single tiles with in a given radius
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public static class GetArea
{
    private static int[] closedSet = new int[5000];
    public static int numUpTo = 0;

    private static List<Tiles> openSet = new List<Tiles>();
    private static List<Tiles> currSet = new List<Tiles>();

    private static List<Tiles> tilesToReturn = new List<Tiles>();

    private static List<Tiles> holder = new List<Tiles>();
    private static List<Tiles> holder2 = new List<Tiles>();

    /*
    * GetAreaOfMoveable
    * public List<Tiles> function (Tiles start, int radius, map)
    * 
    * this function gets all moveable tiles with in the radius given to it
    * around the tile also passed in. once it has all the tiles it returns it as a list
    * 
    * @returns List<Tiles> - all movable tiles
    */
    public static List<Tiles> GetAreaOfMoveable(Tiles start, int radius, Map map, Unit unit)
    {
        //set the closed set back to being -1 by default for logic reason purposes
        for (int i = 0; i < closedSet.Length; i++)
        {
            //we can stop when we hit -1 as there should not be any more changes needed
            if (closedSet[i] == -1)
            {
                break;
            }

            closedSet[i] = -1;
        }

        //start up the open set
        openSet.Clear();
        openSet.Add(start);

        //set numUpto zero for latter logic reasons
        numUpTo = 0;

        //loop through the radius amount and gather all the movable tiles
        for (int i = 0; i <= radius; i++)
        {
            //clear the current set
            currSet.Clear();

            //loop throught the open set and add there tiles into the current set
            for (int u = 0; u < openSet.Count; u++)
            {
                //make sure we are not adding duplicates
                if (FindInContainerCurrent(openSet[u].indexPos) == false)
                {
                    currSet.Add(openSet[u]);
                }
            }
            
            //clear the open set and ready it for new tiles
            openSet.Clear();
            
            //add all the current tiles to the closed set
            for (int u = 0; u < currSet.Count; u++)
            {
                //we use numUpTo here in order to keep track of where in the list we are up to in adding tiles
                closedSet[numUpTo] = currSet[u].indexPos;
                numUpTo++;
            }
            
            //go through all the current tiles and there edges gathering there tiles then add them to the open list
            for (int u = 0; u < currSet.Count; u++)
            {
                for (int y = 0; y < currSet[u].tileEdges.Count; y++)
                {
                    if (currSet[u].tileEdges[y].IsPassible(unit) && FindInContainerClosed(currSet[u].tileEdges[y].indexPos) == false)
                    {
                        openSet.Add(currSet[u].tileEdges[y]);
                    }
                }
            }
        }

        //make sure the list of tiles we want to return is empty
        tilesToReturn.Clear();
        numUpTo = 0;
        //go through the closed list and gather the appropriate tiles from map and add them to the tilesToReturn
        for (int i = 0; i < closedSet.Length; i++)
        {
            if (closedSet[i] == -1)
            {
                break;
            }

            tilesToReturn.Add(map.mapTiles[closedSet[i]]);
            closedSet[i] = -1;
        }
        
        //return the gathered tiles
        return tilesToReturn;
    }

    /*
    * GetAreaOfMoveable
    * public List<Tiles> function (Tiles start, int radius, map)
    * 
    * this function gets all moveable tiles with in the radius given to it
    * around the tile also passed in. once it has all the tiles it returns it as a list
    * 
    * @returns List<Tiles> - all movable tiles
    */
    public static List<Tiles> GetAreaOfMeleeDangerZone(Tiles start, int radius, Map map)
    {
        //set the closed set back to being -1 by default for logic reason purposes
        for (int i = 0; i < closedSet.Length; i++)
        {
            //we can stop when we hit -1 as there should not be any more changes needed
            if (closedSet[i] == -1)
            {
                break;
            }

            closedSet[i] = -1;
        }

        //start up the open set
        openSet.Clear();
        openSet.Add(start);

        //set numUpto zero for latter logic reasons
        numUpTo = 0;

        //loop through the radius amount and gather all the movable tiles
        for (int i = 0; i <= radius; i++)
        {
            //clear the current set
            currSet.Clear();

            //loop throught the open set and add there tiles into the current set
            for (int u = 0; u < openSet.Count; u++)
            {
                //make sure we are not adding duplicates
                if (FindInContainerCurrent(openSet[u].indexPos) == false)
                {
                    currSet.Add(openSet[u]);
                }
            }

            //clear the open set and ready it for new tiles
            openSet.Clear();

            //add all the current tiles to the closed set
            for (int u = 0; u < currSet.Count; u++)
            {
                //we use numUpTo here in order to keep track of where in the list we are up to in adding tiles
                closedSet[numUpTo] = currSet[u].indexPos;
                numUpTo++;
            }

            //go through all the current tiles and there edges gathering there tiles then add them to the open list
            for (int u = 0; u < currSet.Count; u++)
            {
                for (int y = 0; y < currSet[u].tileEdges.Count; y++)
                {
                    if (currSet[u].tileEdges[y].IsPassible() && FindInContainerClosed(currSet[u].tileEdges[y].indexPos) == false)
                    {
                        openSet.Add(currSet[u].tileEdges[y]);
                    }
                }
            }
        }

        //make sure the list of tiles we want to return is empty
        tilesToReturn.Clear();
        numUpTo = 0;
        //go through the closed list and gather the appropriate tiles from map and add them to the tilesToReturn
        for (int i = 0; i < closedSet.Length; i++)
        {
            if (closedSet[i] == -1)
            {
                break;
            }

            tilesToReturn.Add(map.mapTiles[closedSet[i]]);
            closedSet[i] = -1;
        }

        //return the gathered tiles
        return tilesToReturn;
    }

    /*
    * GetAreaOfRangeDangerZone
    * public List<Tiles> function (Tiles start, int radius, map)
    * 
    * this function calculates all the "danger zone" tiles around a unit with a ranged attack
    * basically the area where if you have a unit there unit could attack you from
    * 
    * @returns List<Tiles> - all movable tiles
    */
    public static List<Tiles> GetAreaOfRangeDangerZone(Tiles startTile, int movemantRange, int attackRange, Map map)
    {
        //clear open set
        openSet.Clear();

        //gather all tiles the unit could walk to
        openSet = GetAreaOfMeleeDangerZone(startTile, movemantRange, map);

        holder.Clear();
        holder2.Clear();

        //set the closed set back to being -1 by default for logic reason purposes
        for (int i = 0; i < closedSet.Length; i++)
        {
            //we can stop when we hit -1 as there should not be any more changes needed
            if (closedSet[i] == -1)
            {
                break;
            }

            closedSet[i] = -1;
        }

        currSet.Clear();

        //loop throught the open set and add there tiles into the current set
        for (int u = 0; u < openSet.Count; u++)
        {
            //make sure we are not adding duplicates
            if (FindInContainerCurrent(openSet[u].indexPos) == false)
            {
                currSet.Add(openSet[u]);
            }
        }

        //clear the open set and ready it for new tiles
        openSet.Clear();

        //go through the current tiles looking for the edge tiles and add them to the holder list
        for (int x = 0; x < currSet.Count; x++)
        {
            for (int i = 0; i < currSet[x].tileEdges.Count; i++)
            {
                //if a tiles edge tiles are not in the list then the tile is on the edge of the search
                if (FindInContainerCurrent(currSet[x].tileEdges[i].indexPos) == false)
                {
                    holder.Add(currSet[x]);
                    break;
                }
            }
        }

        for (int i = 0; i < currSet.Count; i++)
        {
            holder2.Add(currSet[i]);
        }

        //go through all the edge tiles and gather the area of attack for earch one
        for (int i = 0; i < holder.Count; i++)
        {
            openSet = GetAreaOfAttack(holder[i], attackRange, map);

            for (int u = 0; u < openSet.Count; u++)
            {
                if (FindInContainerHolder2(openSet[u].indexPos) == false)
                {
                    holder2.Add(openSet[u]);
                }
            }

            openSet.Clear();
        }

        foreach (Tiles tile in holder2)
        {
            Debug.Log(tile.indexPos);
        }

        //return the gathered tiles
        return holder2;
    }

    /*
    * GetAreaOfAttack
    * public List<Tiles> function (Tiles start, int radius, Map map)
    * 
    * this function gets all attackable tiles with in the radius given to it
    * around the tile also passed in. once it has all the tiles it returns it as a list
    * 
    * @returns List<Tiles> - all attackable tiles
    */
    public static List<Tiles> GetAreaOfAttack(Tiles start, int radius, Map map)
    {
        //set the closed set back to being -1 by default for logic reason purposes
        for (int i = 0; i < closedSet.Length; i++)
        {
            //we can stop when we hit -1 as there should not be any more changes needed
            if (closedSet[i] == -1)
            {
                break;
            }

            closedSet[i] = -1;
        }

        //start up the open set
        openSet.Clear();
        openSet.Add(start);

        //set numUpto zero for latter logic reasons
        numUpTo = 0;

        //loop through the radius amount and gather all the movable tiles
        for (int i = 0; i <= radius; i++)
        {
            //clear the current set
            currSet.Clear();

            //loop throught the open set and add there tiles into the current set
            for (int u = 0; u < openSet.Count; u++)
            {
                //make sure we are not adding duplicates
                if (FindInContainerCurrent(openSet[u].indexPos) == false)
                {
                    currSet.Add(openSet[u]);
                }
            }

            //clear the open set and ready it for new tiles
            openSet.Clear();

            //add all the current tiles to the closed set
            for (int u = 0; u < currSet.Count; u++)
            {
                //we use numUpTo here in order to keep track of where in the list we are up to in adding tiles
                closedSet[numUpTo] = currSet[u].indexPos;
                numUpTo++;
            }

            //go through all the current tiles and there edges gathering there tiles then add them to the open list
            for (int u = 0; u < currSet.Count; u++)
            {
                for (int y = 0; y < currSet[u].tileEdges.Count; y++)
                {
                    if (FindInContainerClosed(currSet[u].tileEdges[y].indexPos) == false)
                    {
                        openSet.Add(currSet[u].tileEdges[y]);
                    }
                }
            }
        }

        //make sure the list of tiles we want to return is empty
        tilesToReturn.Clear();
        numUpTo = 0;
        //go through the closed list and gather the appropriate tiles from map and add them to the tilesToReturn
        for (int i = 0; i < closedSet.Length; i++)
        {
            if (closedSet[i] == -1)
            {
                break;
            }

            tilesToReturn.Add(map.mapTiles[closedSet[i]]);
            closedSet[i] = -1;
        }

        //return the gathered tiles
        return tilesToReturn;
    }

    /*
    * FindInContainerCurret
    * public bool function (Int toFind)
    * 
    * this checks the closedSet of values to find duplicates to not add them in
    * 
    * @returns bool
    */
    public static bool FindInContainerClosed(int toFind)
    {
        for (int i = 0; i < closedSet.Length; i++)
        {
            if (closedSet[i] == -1)
            {
                return false;
            }

            if (closedSet[i] == toFind)
            {
                return true;
            }
        }

        return false;
    }


    /*
    * FindInContainerCurret
    * public bool function (Int toFind)
    * 
    * this checks the currSet of tiles to find duplicates to not add them in
    * 
    * @returns bool
    */
    public static bool FindInContainerCurrent(int toFind)
    {
        for (int i = 0; i < currSet.Count; i++)
        {
            if (currSet[i].indexPos == toFind)
            {
                return true;
            }
        }
        return false;
    }

    /*
    * FindInContainerHolder2
    * public bool function (Int toFind)
    * 
    * this checks holder2 of tiles to find duplicates to not add them in
    * 
    * @returns bool
    */
    public static bool FindInContainerHolder2(int toFind)
    {
        for (int i = 0; i < holder2.Count; i++)
        {
            if (holder2[i].indexPos == toFind)
            {
                return true;
            }
        }
        return false;
    }
}
