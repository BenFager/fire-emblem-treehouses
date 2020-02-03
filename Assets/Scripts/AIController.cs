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
    
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        cam = Camera.main.GetComponent<CameraMovement>();
        map = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //AI types:
    // NOTHING, INRANGEWAIT, SQUADWAIT, PURSUETARGET, PURSUENEAREST, PURSUETILE
    //AGGRESSIVE, DEFENSIVE
}
