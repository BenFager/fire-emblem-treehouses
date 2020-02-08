using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    List<MapUnit> units;
    bool isEnabled;
    Cursor cursor;
    CameraMovement cam;
    WorldMap map;
    Pathfinding pathfinding;
    CombatController combatController;
    
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        cam = Camera.main.GetComponent<CameraMovement>();
        map = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
        combatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator moveAI()
    {
        if (combatController.turnType == UnitSide.ENEMY)
        {
            foreach(MapUnit m in combatController.enemyUnits)
            {
                AIUnit currentAI = m.GetComponent<AIUnit>();
                currentAI.TakeTurn();
                while(!m.isDoneMoving())
                {
                    yield return null;
                }
            }
        }
        
    }

    public void Enable(List<MapUnit> AIUnits)
    {
        units = AIUnits;
        isEnabled = true;
        cursor.DisableCursor();
    }
    public void Disable()
    {
        isEnabled = false;
        cursor.Enable();
    }
    public void runAI()
    {
        StartCoroutine(moveAI());
    }

    //AI types:
    // NOTHING, INRANGEWAIT, SQUADWAIT, PURSUETARGET, PURSUENEAREST, PURSUETILE
    //AGGRESSIVE, DEFENSIVE
}
