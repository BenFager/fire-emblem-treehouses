using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMap: MonoBehaviour
{
    public int integer;
    Tilemap tilemapFloor;
    Tilemap tilemapObjects;
    BoundsInt boundsFloor;
    BoundsInt boundsObjects;
    MapTile[,] mapTiles;
    DebugTiles debugTiles;
    Vector2Int lastMouseClick;
    Pathfinding pathfinding;

    enum tileLabel
    {
        FOREST, MOUNTAIN, PLAIN, FORT, NULL
    }
    // Start is called before the first frame update
    void Start()
    {
        tilemapFloor = transform.Find("TilemapFloor").gameObject.GetComponent<Tilemap>();
        tilemapObjects = transform.Find("TilemapObjects").gameObject.GetComponent<Tilemap>();
        
        boundsFloor = tilemapFloor.cellBounds;
        TileBase[] tilesFloor = tilemapFloor.GetTilesBlock(boundsFloor);
        TileBase[] tilesObjects = tilemapObjects.GetTilesBlock(boundsFloor);
        Debug.Log(boundsFloor);


        tilemapObjects = transform.Find("TilemapObjects").gameObject.GetComponent<Tilemap>();
        mapTiles = new MapTile[boundsFloor.size.x / 2, boundsFloor.size.y / 2];
        for(int x = 0; x < boundsFloor.size.x / 2; x++)
        {
            for(int y = 0; y < boundsFloor.size.y/2; y++)
            {
                mapTiles[x, y] = new MapTile();
                mapTiles[x, y].floorData = mapTileToFloor(tilesFloor[(y * boundsFloor.size.x + x) * 2]?.name);
                mapTiles[x, y].mapObjectData = mapTileToObject(tilesObjects[(y * boundsFloor.size.x + x) * 2]?.name);
                
            }
        }

        debugTiles = transform.Find("TilemapDebug").GetComponent<DebugTiles>();
        pathfinding = GetComponent<Pathfinding>();

    }

    // Update is called once per frame---------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int arrayMousePos = WorldToArrayPos(mousePos);
            lastMouseClick = arrayMousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ClearDebugLines();
            ClearDebugTiles();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int arrayMousePos = WorldToArrayPos(mousePos);
            List<Vector2Int> path = pathfinding.GetAIPath(mapTiles, lastMouseClick, arrayMousePos, 5);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {

                    SetDebugLine(path[i], path[i + 1]);
                }
            }

        }

    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

    Vector2 arrayPosToWorld(Vector2Int input)
    {
        Vector2Int floorToBounds = new Vector2Int(IntFloor(boundsFloor.min.x, 2), IntFloor(boundsFloor.min.y, 2));
        return tilemapFloor.CellToWorld((Vector3Int)input * 2 + (Vector3Int)(floorToBounds));
    }
    Vector2Int WorldToArrayPos(Vector2 input)
    {
        Vector2Int floorToBounds = new Vector2Int(IntFloor(boundsFloor.min.x, 2), IntFloor(boundsFloor.min.y, 2));
        Vector3Int v = (tilemapFloor.WorldToCell(input) - (Vector3Int)floorToBounds);
        //Debug.Log(tilemapFloor.WorldToCell(input));
        //Debug.Log(v);
        //Debug.Log(new Vector2Int(v.x / 2, v.y / 2));
        return new Vector2Int(v.x / 2, v.y / 2);

    }
    floor mapTileToFloor(string s)
    {
        if(s == null)
        {
            return floor.NULL;
        }
        int id = Int32.Parse(s.Replace("Floor_", ""));
        switch (id)
        {
            case 0:
            case 1:
            case 10:
            case 11:
                return floor.FOREST;
            case 2:
            case 3:
            case 12:
            case 13:
                return floor.FORT;
            case 4:
            case 5:
            case 14:
            case 15:
                return floor.PLAIN;
            case 6:
            case 7:
            case 16:
            case 17:
                return floor.MOUNTAIN;
            case 8:
            case 9:
            case 18:
            case 19:
                return floor.FLOOR;
            default:
                return floor.NULL;
        }
    }
    mapObject mapTileToObject(string s)
    {
        if(s == null)
        {
            return mapObject.NULL;
        }
        int id = Int32.Parse(s.Replace("Objects_", ""));
        switch (id)
        {
            case 0:
            case 1:
            case 12:
            case 13:
                return mapObject.BLCORNER;
            case 2:
            case 3:
            case 14:
            case 15:
                return mapObject.BRCORNER;
            case 4:
            case 5:
            case 16:
            case 17:
                return mapObject.HORIZWALL;
            case 6:
            case 7:
            case 18:
            case 19:
                return mapObject.TLCORNER;
            case 8:
            case 9:
            case 20:
            case 21:
                return mapObject.TRCORNER;
            case 10:
            case 11:
            case 22:
            case 23:
                return mapObject.HOUSE;
            case 24:
            case 25:
            case 28:
            case 29:
                return mapObject.VERTWALL;
            default:
                return mapObject.NULL;
        }

    }
    public static int IntFloor(int value, int multiple)
        {
            multiple = Math.Max(1, Math.Abs(multiple));
            return multiple * (int)Math.Floor((float)value / multiple);
        }

    public void SetDebugTile(Vector2Int arrayPos, Color c)
    {
        debugTiles.SetTile(arrayPosToWorld(arrayPos), c);
    }
    public void ClearDebugTiles()
    {
        debugTiles.ClearTiles();
    }
    public void SetDebugLine(Vector2Int arrayPosA, Vector2Int arrayPosB)
    {
        debugTiles.SetLine(arrayPosToWorld(arrayPosA) + (Vector2)tilemapFloor.cellSize, arrayPosToWorld(arrayPosB) + (Vector2)tilemapFloor.cellSize);
    }
    public void ClearDebugLines()
    {
        debugTiles.ClearLines();
    }
}

public enum floor
{
    FOREST, MOUNTAIN, PLAIN, FORT, FLOOR, NULL
}
public enum mapObject
{
    HORIZWALL, VERTWALL, TLCORNER, TRCORNER, BLCORNER, BRCORNER, HOUSE, NULL
}

public class MapTile
{
    

    public floor floorData;
    public mapObject mapObjectData;

    public override string ToString()
    {
        return floorData.ToString() + " " + mapObjectData.ToString();
    }
    public Boolean GetPassable()
    {
        if((mapObjectData != mapObject.HOUSE && mapObjectData != mapObject.NULL) || floorData == floor.NULL)
        {
            return false;
        }
        else
        {
            return true;
        }


    }
    public float GetCost()
    {
        switch (floorData)
        {
            case floor.FOREST:
                return 2;
            case floor.MOUNTAIN:
                return 3;
            case floor.PLAIN:
                return 1;
            case floor.FORT:
                return 2;
            case floor.FLOOR:
                return 1;
            default:
                return 1;
        }

    }
}

