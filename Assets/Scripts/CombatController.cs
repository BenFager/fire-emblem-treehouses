using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//I AM YOUR GOD BOW DOWN TO MEEEEEEEEE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Pick a godObject and pray
public class CombatController : MonoBehaviour
{
    public UnitSide turnType;
    public List<MapUnit> enemyUnits;
    public List<MapUnit> allyUnits;
    public List<MapUnit> playerUnits;
    PlayerController playerController;
    Coroutine c;
    // Start is called before the first frame update
    public void Initialize()
    {
        turnType = UnitSide.PLAYER;
        playerController = GetComponent<PlayerController>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (c == null)
        {
            c = StartCoroutine(combatLoop());
        }
    }

    public IEnumerator combatLoop()
    {
        while (true)
        {
            switch (turnType)
            {
                case UnitSide.PLAYER:
                    Debug.Log("Player phase!");
                    foreach (MapUnit c in playerUnits)
                    {
                        c.EnableAction();
                        //Todo: enable player UI
                    }
                    playerController.Enable(playerUnits);
                    while (anyHaveTurn(playerUnits))
                    {
                        yield return null;
                    }
                    turnType = UnitSide.ENEMY;
                    playerController.Disable();
                    Debug.Log("player phase done 2");
                    //close player ui
                    break;
                case UnitSide.ENEMY:
                    Debug.Log("Enemy Turn!");//Todo: replace with ui
                    foreach (MapUnit c in enemyUnits)   
                    {
                        c.EnableAction();
                        //Todo: enable enemy ai controller UI
                    }
                    while (anyHaveTurn(enemyUnits))
                    {
                        yield return null;
                    }
                    //Notify enemy ai controller that it's done
                    turnType = UnitSide.ALLY;
                    break;
                case UnitSide.ALLY:

                    Debug.Log("Ally Turn!");//Todo: replace with ui
                    foreach (MapUnit c in allyUnits)
                    {
                        c.EnableAction();
                        //Todo: enable ally ai controller UI
                    }
                    while (anyHaveTurn(allyUnits))
                    {
                        yield return null;
                    }
                    //Notify ally ai controller that it's done
                    turnType = UnitSide.PLAYER;
                    break;

            }
            yield return null;
        }
    }

    bool anyHaveTurn(List<MapUnit> units)
    {
        bool tempIsFinished = true;
        foreach (MapUnit c in units)
        {
            if (c.hasTurn)
            {
                tempIsFinished = false;
            }
        }
        return (!tempIsFinished);
    }

    CombatController GetInstance()
    {
        return GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
    }

    public UnitSide getSide(MapUnit m)
    {
        foreach(MapUnit enemyUnit in enemyUnits)
        {
            if(m == enemyUnit)
            {
                return UnitSide.ENEMY;
            }
        }
        foreach(MapUnit allyUnit in allyUnits)
        {
            if(m == allyUnit)
            {
                return UnitSide.ALLY;
            }
        }
        return UnitSide.PLAYER;
    }
}
public enum UnitSide
{
    PLAYER, ENEMY, ALLY
}