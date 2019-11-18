using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMap: MonoBehaviour
{

    public BoundsInt mapBounds;
    Tilemap tilemapFloor;
    Tilemap tilemapObjects;
    BoundsInt boundsFloor;
    BoundsInt boundsObjects;
    public MapTile[,] mapTiles;
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
        int xMin = mapTiles.GetLength(0);
        int yMin = mapTiles.GetLength(0);
        int xMax = -1;
        int yMax = -1;
        for(int x = 0; x < mapTiles.GetLength(0); x++)
        {
            for(int y = 0; y < mapTiles.GetLength(1); y++)
            {
                if(mapTiles[x,y].floorData != Floor.NULL)
                {
                    xMin = Math.Min(xMin, x);
                    yMin = Math.Min(yMin, y);
                    xMax = Math.Max(xMax, x);
                    yMax = Math.Max(yMax, y);
                }
            }
        }
        xMax += 1;
        yMax += 1;
        mapBounds.xMax = xMax;
        mapBounds.xMin = xMin;
        mapBounds.yMax = yMax;
        mapBounds.yMin = yMin;

    }

    // Update is called once per frame---------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
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

        }*/

    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public Vector2 arrayPosToWorld(Vector2Int input)
    {
        Vector2Int floorToBounds = new Vector2Int(IntFloor(boundsFloor.min.x, 2), IntFloor(boundsFloor.min.y, 2));
        return tilemapFloor.CellToWorld((Vector3Int)input * 2 + (Vector3Int)(floorToBounds));
    }
    public Vector2Int WorldToArrayPos(Vector2 input)
    {
        Vector2Int floorToBounds = new Vector2Int(IntFloor(boundsFloor.min.x, 2), IntFloor(boundsFloor.min.y, 2));
        Vector3Int v = (tilemapFloor.WorldToCell(input) - (Vector3Int)floorToBounds);
        //Debug.Log(tilemapFloor.WorldToCell(input));
        //Debug.Log(v);
        //Debug.Log(new Vector2Int(v.x / 2, v.y / 2));
        return new Vector2Int(v.x / 2, v.y / 2);

    }
    public Vector2 arrayCellSize()
    {
        return 2 * tilemapFloor.cellSize;
    }
    Floor mapTileToFloor(string s)
    {
        if(s == null)
        {
            return Floor.NULL;
        }
        int id = Int32.Parse(s.Replace("Floor_", ""));
        switch (id)
        {
            case 0:
            case 1:
            case 10:
            case 11:
                return Floor.FOREST;
            case 2:
            case 3:
            case 12:
            case 13:
                return Floor.FORT;
            case 4:
            case 5:
            case 14:
            case 15:
                return Floor.PLAIN;
            case 6:
            case 7:
            case 16:
            case 17:
                return Floor.MOUNTAIN;
            case 8:
            case 9:
            case 18:
            case 19:
                return Floor.FLOOR;
            default:
                return Floor.NULL;
        }
    }
    MapObject mapTileToObject(string s)
    {
        if(s == null)
        {
            return MapObject.NULL;
        }
        int id = Int32.Parse(s.Replace("Objects_", ""));
        switch (id)
        {
            case 0:
            case 1:
            case 12:
            case 13:
            case 2:
            case 3:
            case 14:
            case 15:
            case 4:
            case 5:
            case 16:
            case 17:
            case 6:
            case 7:
            case 18:
            case 19:
            case 8:
            case 9:
            case 20:
            case 21:
            case 24:
            case 25:
            case 28:
            case 29:
                return MapObject.WALL;
            case 10:
            case 11:
            case 22:
            case 23:
                return MapObject.HOUSE;
            default:
                return MapObject.NULL;
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

public enum Floor
{
    FOREST, MOUNTAIN, PLAIN, FORT, FLOOR, NULL
}
public enum MapObject
{
    WALL, HOUSE, NULL, TALLWALL
}

public class MapTile
{
    

    public Floor floorData;
    public MapObject mapObjectData;

    public override string ToString()
    {
        return floorData.ToString() + " " + mapObjectData.ToString();
    }
    public Boolean GetPassable(MapUnit c)
    {
        if(floorData == Floor.NULL)
        {
            return false;
        }
        if((mapObjectData != MapObject.HOUSE && mapObjectData != MapObject.NULL))
        {
            
            if(c.movementType == MovementType.FLIER)
            {
                return mapObjectData != MapObject.TALLWALL;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }


    }
    public float GetCost(MapUnit c)
    {
        if(c.movementType == MovementType.FLIER)
        {
            return 1;
        }
        if (c.movementType == MovementType.GROUNDED)
        {
            switch (floorData)
            {
                case Floor.FOREST:
                    return 2;
                case Floor.MOUNTAIN:
                    return 3;
                case Floor.PLAIN:
                    return 1;
                case Floor.FORT:
                    return 2;
                case Floor.FLOOR:
                    return 1;
                default:
                    return 1;
            }
        }
        if(c.movementType == MovementType.MOUNTED)
        {
            switch (floorData)
            {
                case Floor.FOREST:
                    return 4;
                case Floor.MOUNTAIN:
                    return 6;
                case Floor.PLAIN:
                    return 1;
                case Floor.FORT:
                    return 3;
                case Floor.FLOOR:
                    return 1;
                default:
                    return 1;
            }
        }
        return 1;

    }
}

