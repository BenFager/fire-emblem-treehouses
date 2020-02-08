using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ApproachAI : MonoBehaviour, AIUnit
{

    Pathfinding pathfinding;
    WorldMap worldMap;
    Coroutine c;
    CombatController combatController;
    MapUnit thisUnit;
    bool isFinishedWithTurn;
    int tempCounter = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        worldMap = WorldMap.GetInstance();
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
        combatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        thisUnit = GetComponent<MapUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator runApproachAI()
    {
        isFinishedWithTurn = false;
        bool targetsInRange = false;
        Vector2Int locationToTravelTo;
        Vector2Int locationToAttack = new Vector2Int(int.MaxValue, int.MaxValue);
        if (combatController.getSide(thisUnit) == UnitSide.ENEMY)
        {
            if (thisUnit.getUnitsInRange().Any(x => x.getSide() == UnitSide.ALLY) || thisUnit.getUnitsInRange().Any(x => x.getSide() == UnitSide.PLAYER))
            {
                targetsInRange = true;
            }
        }
        else
        {
            if(thisUnit.getUnitsInRange().Any(x => x.getSide() == UnitSide.ENEMY))
            {
                targetsInRange = true;
            }
        }
        if (!targetsInRange)//No target in range------------------------------------------------------------------------------------------------------------------------
        {
            
            //Calculate nearest unit
            UnitSide mySide = combatController.getSide(thisUnit);
            List<UnitAndCost> potentialTargets = new List<UnitAndCost>();

            //Going to have to change this so that it is ordered

            if (mySide == UnitSide.ENEMY)
            {
                for (int i = 0; i < combatController.allyUnits.Count; i++)
                {

                    List<PathNode> listOfCurrentNodes = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, combatController.allyUnits[i].loc, thisUnit);
                    if (listOfCurrentNodes != null)
                    {
                        float cost = listOfCurrentNodes[listOfCurrentNodes.Count - 1].cost;
                        potentialTargets.Add(new UnitAndCost(combatController.allyUnits[i], cost, UnitSide.ALLY));
                    }
                }
                for (int i = 0; i < combatController.playerUnits.Count; i++)
                {

                    List<PathNode> listOfCurrentNodes = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, combatController.playerUnits[i].loc, thisUnit);
                    if (listOfCurrentNodes != null)
                    {
                        float cost = listOfCurrentNodes[listOfCurrentNodes.Count - 1].cost;
                        potentialTargets.Add(new UnitAndCost(combatController.playerUnits[i], cost, UnitSide.PLAYER));
                    }
                }
            }
            else
            {
                for (int i = 0; i < combatController.enemyUnits.Count; i++)
                {

                    List<PathNode> listOfCurrentNodes = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, combatController.enemyUnits[i].loc, thisUnit);
                    if (listOfCurrentNodes != null)
                    {
                        float cost = listOfCurrentNodes[listOfCurrentNodes.Count - 1].cost;
                        potentialTargets.Add(new UnitAndCost(combatController.enemyUnits[i], cost, UnitSide.ENEMY));
                    }
                }

            }

            //SORT HERE---------------------------------------------------

            potentialTargets.Sort(new UnitCostSorter());



            //-------------------------------------------------------------

            if (potentialTargets.Count == 0)
            {
                //DO NOTHING, NOTHING CAN BE REACHED
                //NOTHING I TELL YOU
                locationToTravelTo = thisUnit.loc;
            }
            else
            {
                locationToTravelTo = thisUnit.loc;
                foreach(UnitAndCost currentUnit in potentialTargets)
                {
                    List<Vector2Int> tilesICanPathTo = new List<Vector2Int>();
                    foreach(Vector2Int v in currentUnit.m_unit.getAttackTiles(true))
                    {
                        if(pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, v, thisUnit) != null && combatController.unitInTile(v) == null)
                        {
                            tilesICanPathTo.Add(v);
                        }
                    }
                    if(!(tilesICanPathTo.Count > 0))
                    {
                        locationToAttack = currentUnit.m_unit.loc;
                        if (tilesICanPathTo.Count == 1) //there's one viable location
                        {
                            locationToTravelTo = tilesICanPathTo[0];
                        }
                        else //there's more than one viable location
                        {
                            int indexOfShortest = 0;
                            List<PathNode> nodesAtFirst = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, tilesICanPathTo[0], thisUnit);
                            float costOfShortest = nodesAtFirst[nodesAtFirst.Count - 1].cost;
                            for(int i = 1; i < tilesICanPathTo.Count; i++)
                            {
                                
                                List<PathNode> nodes = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, tilesICanPathTo[i], thisUnit);
                                if (nodes[nodes.Count - 1].cost < costOfShortest)
                                {
                                    indexOfShortest = i;
                                    costOfShortest = nodes[nodes.Count - 1].cost;
                                }
                            }
                            locationToTravelTo = tilesICanPathTo[indexOfShortest];
                        }
                        break;
                    }
                }
                if(locationToTravelTo == thisUnit.loc)
                {
                    //if we can't reach anything, just approach the nearest enemy(or nearest spot in range of the nearest enemy)
                    List<PathNode> nodesToBacktrack = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, potentialTargets[0].m_unit.loc, thisUnit);
                    bool isFinished = false;
                    while(nodesToBacktrack.Count > 0 && !isFinished)
                    {
                        if(combatController.unitInTile(nodesToBacktrack[nodesToBacktrack.Count - 1].pos) != null)
                        {
                            nodesToBacktrack.Remove(nodesToBacktrack[nodesToBacktrack.Count - 1]);
                        }
                        else
                        {
                            isFinished = true;
                        }
                    }
                    locationToTravelTo = nodesToBacktrack[nodesToBacktrack.Count - 1].pos;
                }

                //we now have a target selected
            }

            
        }
        else//There are targets in range--------------------------------------------------------------------------------------
        {
            Debug.Log("I've got you in MY SIGHTS");
            locationToTravelTo = thisUnit.loc;
        }
        //Move to the targeted location----------
        //while not at target
        //{
        //move a little
        Debug.Log("Got to point 1" + tempCounter);
        thisUnit.Move(pathfinding.GetAIPath(worldMap.mapTiles, thisUnit.loc, locationToTravelTo, thisUnit.numOfMovement, thisUnit).Select(x => x.pos).ToList());
        Debug.Log("Got to point 2" + tempCounter);
        while(!thisUnit.isDoneMoving())
        {
            Debug.Log("Is not done moving" + tempCounter);
            tempCounter++;
            yield return null;
        }
        if (targetsInRange)
        {
            thisUnit.Attack(combatController.unitInTile(locationToAttack));
        }
        //}
        //once at targeted location, if(targetsInRange) attack target at locationToAttack
        isFinishedWithTurn = true;
    }

    public void TakeTurn()
    {
        StartCoroutine(runApproachAI());
    }

    public bool isFinished()
    {
        return isFinishedWithTurn;
    }
}

public class UnitAndCost
{
    public MapUnit m_unit;
    public float m_cost;
    public UnitSide m_alliegance;
    public UnitAndCost(MapUnit unit, float cost, UnitSide alliegance)
    {
        m_unit = unit;
        m_cost = cost;
        m_alliegance = alliegance;
    }
    public UnitAndCost(UnitAndCost toCopy)
    {
        m_unit = toCopy.m_unit;
        m_cost = toCopy.m_cost;
        m_alliegance = toCopy.m_alliegance;
    }
    public void copy(UnitAndCost toCopy)
    {
        m_unit = toCopy.m_unit;
        m_cost = toCopy.m_cost;
        m_alliegance = toCopy.m_alliegance;
    }
}

public class UnitCostSorter : IComparer<UnitAndCost>
{
    public int Compare(UnitAndCost x, UnitAndCost y)
    {
        if (x.m_cost == y.m_cost)
        {
            return 0;
        }
        else if (x.m_cost < y.m_cost)
        {
            return -1;
        }
        return 1;
    }
}
