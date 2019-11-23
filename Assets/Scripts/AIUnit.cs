using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : MonoBehaviour
{
    Target target;
    MapUnit thisUnit;
    List<AIType> AICommands;
    MapUnit mapUnit;



    public void Start()
    {
        
    }
    public void Update()
    {
        
    }


}

public enum AIType  //Nothing is stand still and wait, INRANGEWAIT is waiting until enemy unit is in range, SQUADWAIT is waiting until enemy is in range of anyone in squad
                       //PURSUENEAREST is always attacking whichever enemy is the nearest, PURSUETILE tries to go to a tile: if aggressive, they get distracted if an enemy is in range, if defensive they only approach their target
{
    NOTHING, INRANGEWAIT, SQUADWAIT, PURSUETARGET, PURSUENEAREST, PURSUETILE
}
public enum AIPreferences  //This will determine whether the unit will go back to a fort when in wait or pursue mode
{
    AGGRESSIVE, DEFENSIVE
}

public class AIDataStorer
{
    public AIType type;
    public AIPreferences preferences;
}



public interface Target
{
    Vector2Int getLocation();
}

public class UnitTarget : Target
{
    MapUnit targetUnit;
    public UnitTarget(MapUnit targetUnit)
    {
        this.targetUnit = targetUnit;
    }
    public Vector2Int getLocation()
    {
        return targetUnit.loc;
    }
}
public class LocationTarget : Target
{
    Vector2Int locationOnMap;
    public LocationTarget(Vector2Int location)
    {
        locationOnMap = location;
    }

    public Vector2Int getLocation()
    {
        return locationOnMap;
    }
}
