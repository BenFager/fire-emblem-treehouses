using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    Cursor cursor;
    CameraMovement cam;
    WorldMap map;
    bool isEnabled;
    State state;
    List<MapUnit> units;
    MapUnit selectedUnit;
    List<Vector2Int> potentialPaths;
    Pathfinding pathfinding;
    Vector2Int selectedTile;
    List<Vector2Int> selectedPath;
    Vector2Int previousCursorPos = new Vector2Int(-1, -1);
    // Start is called before the first frame update
    public void Initialize()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        cam = Camera.main.GetComponent<CameraMovement>();
        map = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        state = State.SELECTPLAYER;
        pathfinding = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            switch (state)
            {
                case State.SELECTPLAYER:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        previousCursorPos = new Vector2Int(-1, -1);
                        //Checks to see if the cursor is placed on top of a player, if it is, then it sets selectedUnit
                        foreach (MapUnit unit in units)
                        {
                            if (cursor.pos == unit.loc && unit.hasTurn)
                            {
                                selectedUnit = unit;
                                state = State.SELECTPOSITION;
                                potentialPaths = pathfinding.GetPaths(map.mapTiles, unit.loc, unit.numOfMovement, unit).Select(x => x.pos).ToList();
                                foreach (Vector2Int v in potentialPaths)
                                {
                                    map.SetDebugTile(v, Color.blue);
                                }
                            }
                        }
                    }
                    if(Input.GetKeyDown(KeyCode.A))
                    {
                        List<MapUnit> unitsWithTurn = new List<MapUnit>();
                        foreach (MapUnit unit in units)
                        {
                            
                            if(unit.hasTurn)
                            {
                                unitsWithTurn.Add(unit);
                            }
                        }
                        if (unitsWithTurn.Any(x => x.loc == cursor.pos))
                        {
                            int indexToMoveTo = 0;
                            for(int i = 0; i < unitsWithTurn.Count; i++)
                            {
                                if(unitsWithTurn[i].loc == cursor.pos)
                                {
                                    if(i + 1 < unitsWithTurn.Count)
                                    {
                                        indexToMoveTo = i + 1;
                                    }
                                    else
                                    {
                                        indexToMoveTo = 0;
                                    }
                                }
                            }
                            cursor.pos = unitsWithTurn[indexToMoveTo].loc;
                        }
                        else
                        {
                            cursor.pos = unitsWithTurn[0].loc;
                        }
                    }
                    break;

                case State.SELECTPOSITION:
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        state = State.SELECTPLAYER;
                        map.ClearDebugTiles();
                        map.ClearDebugLines();
                    }
                    else
                    {
                        if (potentialPaths.Any(x => x == cursor.pos) )
                        {
                            //Draw debug lines
                            if(previousCursorPos != cursor.pos)
                            {
                                //Reset previous cursor pos, reset debug lines, draw debug lines
                                previousCursorPos = cursor.pos;
                                selectedPath = pathfinding.Pathfind(map.mapTiles, selectedUnit.loc, cursor.pos, selectedUnit).Select(x => x.pos).ToList();
                                map.ClearDebugLines();
                                
                                if(selectedPath != null && selectedPath.Count > 1)
                                {
                                    for(int i = 1; i < selectedPath.Count; i++)
                                    {

                                        map.SetDebugLine(selectedPath[i - 1], selectedPath[i]);
                                    }
                                }
                            }

                            if (Input.GetKeyDown(KeyCode.Z))
                            { 
                                
                                selectedTile = cursor.pos;
                                state = State.MOVING;
                                //selectedPath = pathfinding.Pathfind(map.mapTiles, selectedUnit.loc, selectedTile, selectedUnit);
                                selectedUnit.Move(selectedPath);
                                map.ClearDebugTiles();
                                map.ClearDebugLines();

                            }
                            
                        }
                    }

                    break;
                case State.MOVING:
                    if (selectedUnit.path == null)
                    {
                        state = State.SELECTPLAYER;
                        selectedUnit.DisableAction();
                    }
                    break;
            }
        }
    }

    public void Enable(List<MapUnit> units)
    {
        cursor.pos = units[0].loc;
        cam.cameraPos = units[0].loc;
        cursor.Enable();
        isEnabled = true;
        state = State.SELECTPLAYER;
        this.units = units;
    }
    public void Disable()
    {
        cursor.DisableCursor();
        isEnabled = false;
        Debug.Log("player phase done");
    }


}
enum State
{
    SELECTPLAYER, SELECTPOSITION, MOVING
}