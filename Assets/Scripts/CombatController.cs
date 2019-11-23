using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//I AM YOUR GOD BOW DOWN TO MEEEEEEEEE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//Pick a godObject and pray
public class CombatController : MonoBehaviour
{
    public TurnType turnType;
    public List<MapUnit> enemyUnits;
    public List<MapUnit> allyUnits;
    public List<MapUnit> playerUnits;
    PlayerController playerController;
    Coroutine c;
    // Start is called before the first frame update
    public void Initialize()
    {
        turnType = TurnType.PLAYER;
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
                case TurnType.PLAYER:
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
                    turnType = TurnType.ENEMY;
                    playerController.Disable();
                    Debug.Log("player phase done 2");
                    //close player ui
                    break;
                case TurnType.ENEMY:
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
                    turnType = TurnType.ALLY;
                    break;
                case TurnType.ALLY:

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
                    turnType = TurnType.PLAYER;
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
}
public enum TurnType
{
    PLAYER, ENEMY, ALLY
}
