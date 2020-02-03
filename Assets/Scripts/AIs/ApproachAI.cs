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
    

    // Start is called before the first frame update
    void Start()
    {
        worldMap = WorldMap.GetInstance();
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
        combatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator runApproachAI()
    {
        bool targetsInRange = false;
        Vector2Int locationToTravelTo;
        Vector2Int locationToAttack;
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
            

            int arrayIndexOfClosest = 0;
            UnitSide sideBeingTargeted = UnitSide.ENEMY;
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
                        //if (cost < distanceToClosest)
                        //{
                        //    arrayIndexOfClosest = i;
                        //    distanceToClosest = cost;
                        //    sideBeingTargeted = UnitSide.ALLY;
                        //}
                    }
                }
                for (int i = 0; i < combatController.playerUnits.Count; i++)
                {

                    List<PathNode> listOfCurrentNodes = pathfinding.Pathfind(worldMap.mapTiles, thisUnit.loc, combatController.playerUnits[i].loc, thisUnit);
                    if (listOfCurrentNodes != null)
                    {
                        float cost = listOfCurrentNodes[listOfCurrentNodes.Count - 1].cost;
                        potentialTargets.Add(new UnitAndCost(combatController.playerUnits[i], cost, UnitSide.PLAYER));
                        //if (cost < distanceToClosest)
                        //{
                        //    arrayIndexOfClosest = i;
                        //    distanceToClosest = cost;
                        //    sideBeingTargeted = UnitSide.PLAYER;
                        //}
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
                        //if (cost < distanceToClosest)
                        //{
                        //    arrayIndexOfClosest = i;
                        //    distanceToClosest = cost;
                        //    sideBeingTargeted = UnitSide.ENEMY;
                        //}
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
            }
            else
            {
                MapUnit unitBeingTargeted;
                if (sideBeingTargeted == UnitSide.ALLY)
                {
                    unitBeingTargeted = combatController.allyUnits[arrayIndexOfClosest];
                }
                else if (sideBeingTargeted == UnitSide.ENEMY)
                {
                    unitBeingTargeted = combatController.enemyUnits[arrayIndexOfClosest];
                }
                else
                {
                    unitBeingTargeted = combatController.playerUnits[arrayIndexOfClosest];
                }
                //Find which location closest to it is 
            }

            
        }
        else//There are targets in range--------------------------------------------------------------------------------------
        {

        }
        //Move to the targeted location----------
        //while not at target
        //{
        //move a little
        yield break;
        //}
        //once at targeted location, if(targetsInRange) attack target at locationToAttack
    }

    public void TakeTurn()
    {
        StartCoroutine(runApproachAI());
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
