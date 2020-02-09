using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MapUnit : MonoBehaviour
{
    Animator animator;
    public MovementType movementType;
    public int numOfMovement;
    public Vector2Int loc;
    WorldMap worldMap;
    public bool hasTurn = true;
    public List<Vector2Int> path;
    public CombatController combatController;
    Pathfinding pathfinding;
    PlayerController playerController;
    public AIUnit aiUnit;


    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        worldMap = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        loc = worldMap.WorldToArrayPos(transform.position);
        transform.position = worldMap.arrayPosToWorld(loc) + new Vector2(0.5f, 0.5f);
        combatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    public void Update()
    {
        animator.SetBool("active", hasTurn);

        if(path != null)//or boolean like moving
        {
            //check if path.count > 1, 
            if(path.Count <= 1)
            {
                path = null;
            }
            else
            {
                Vector2 nextPoint = worldMap.arrayPosToWorld(path[1]) + new Vector2(0.5f, 0.5f);
                Vector2 currentPos = Vector2.MoveTowards(transform.position, nextPoint, 10 * Time.deltaTime /*mach 5*/);
                if(currentPos == nextPoint)
                {
                    path.RemoveAt(0);
                }
                transform.position = currentPos;
                if(path.Count <= 1)
                {
                    //Tell playercontroller that we're done moving
                    loc = path[0];
                }
            }
        }

    }
    public void Move(List<Vector2Int> path)
    {
        this.path = new List<Vector2Int>(path);
    }
    public void UseItem()
    {

    }
    public void Attack(MapUnit target)
    {

    }
    public void EnableAction()
    {
        //animator.SetBool("active", true);
        hasTurn = true;
    }
    public void DisableAction()
    {
        //animator.SetBool("active", false);
        hasTurn = false;
    }
    public bool isDoneMoving()
    {
        return path == null;

    }

    //In the future this function will get attack ranges based on inventory/weapon ranges
    public List<Vector2Int> getAttackTiles(MapUnit m, Vector2Int pos, bool isPathfinding)
    {
        //List<int> attackRanges;
        List<Vector2Int> currentLocations;
        List<Vector2Int> returnableLocations;
        //List<Vector2Int> pastLocations;
        //int level = 0;
        //foreach
        returnableLocations = new List<Vector2Int>();
        currentLocations = new List<Vector2Int>();
        //check inventory here-----------------------------------------------------(this is why we need MapUnit m)
        currentLocations.Add(pos + new Vector2Int(0, 1));
        currentLocations.Add(pos + new Vector2Int(1, 0));
        currentLocations.Add(pos + new Vector2Int(0, -1));
        currentLocations.Add(pos + new Vector2Int(-1, 0));
        //------------------------------------------------------------------
        if(isPathfinding)//only add the ones that you can get to
        {
            foreach(Vector2Int v in currentLocations)
            {
                //if the tile is passable and
                if(worldMap.mapTiles[v.x, v.y].GetPassable(this) &&
                   //no enemy, ally, or player units that are in the location that is not itself
                   !combatController.enemyUnits.Any(x => x.loc == v && x != this) &&
                   !combatController.allyUnits.Any(x => x.loc == v && x != this) &&
                   !combatController.playerUnits.Any(x=> x.loc == v && x != this))
                {
                    returnableLocations.Add(v);
                }
            }
        }
        
        else//do all of them
        {
            foreach(Vector2Int v in currentLocations)
            {
                returnableLocations.Add(v);
            }
        }


        return returnableLocations;
    }
    public List<Vector2Int> getAttackTiles(bool isPathfinding)
    {
        return (getAttackTiles(this, this.loc, isPathfinding));
    }
    public List<MapUnit> getUnitsInRange()
    {
        List<MapUnit> returnList = new List<MapUnit>();
        List<PathNode> tilesCanWalkTo = pathfinding.GetPaths(worldMap.mapTiles, this.loc, this.numOfMovement, this);
        List<Vector2Int> tilesInRange = new List<Vector2Int>();
        foreach(PathNode p in tilesCanWalkTo)
        {
            
            foreach(Vector2Int v in getAttackTiles(this, p.pos, false))
            {
                if(!tilesInRange.Any(x => v == x))
                {
                    tilesInRange.Add(v);
                }
            }
            
        }
        foreach (MapUnit m in combatController.playerUnits)
        {
            if(tilesInRange.Any(x => x.x == m.loc.x && x.y == m.loc.y))
            {
                returnList.Add(m);
            }

        }
        return(returnList);
    }




    public UnitSide getSide()
    {
        return combatController.getSide(this);
    }
}
public enum MovementType
{
    MOUNTED, GROUNDED, FLIER
}


public interface AIUnit
{
    
    void TakeTurn();
    bool isFinished();
}
