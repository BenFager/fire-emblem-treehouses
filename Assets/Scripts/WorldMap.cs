using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMap: MonoBehaviour
{

    public BoundsInt mapBounds;
    public Tilemap tilemapReal;
    BoundsInt boundsFloor;
    BoundsInt boundsObjects;
    public MapTile[,] mapTiles;
    public DebugTiles debugTiles;
    Vector2Int lastMouseClick;
    Pathfinding pathfinding;

    enum tileLabel
    {
        FOREST, MOUNTAIN, PLAIN, FORT, NULL
    }
    // Start is called before the first frame update
    public void /*Fire emblem */Initialize/*ning*/()
    {
        tilemapReal = transform.Find("TilemapReal").gameObject.GetComponent<Tilemap>();
        tilemapReal.color = Color.clear;
        boundsFloor = tilemapReal.cellBounds;
        TileBase[] tilesWorld = tilemapReal.GetTilesBlock(boundsFloor);
        Debug.Log(boundsFloor);

        
        mapTiles = new MapTile[boundsFloor.size.x / 2, boundsFloor.size.y / 2];
        for(int x = 0; x < boundsFloor.size.x / 2; x++)
        {
            for(int y = 0; y < boundsFloor.size.y/2; y++)
            {
                mapTiles[x, y] = new MapTile();
                mapTiles[x, y].tile = parseMapTile(tilesWorld[(y * boundsFloor.size.x + x) * 2]?.name);
                
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
                if(mapTiles[x,y].tile != TileType.NULL)
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
        return tilemapReal.CellToWorld((Vector3Int)input * 2 + (Vector3Int)(floorToBounds));
    }
    public Vector2Int WorldToArrayPos(Vector2 input)
    {
        Vector2Int floorToBounds = new Vector2Int(IntFloor(boundsFloor.min.x, 2), IntFloor(boundsFloor.min.y, 2));
        Vector3Int v = (tilemapReal.WorldToCell(input) - (Vector3Int)floorToBounds);
        //Debug.Log(tilemapFloor.WorldToCell(input));
        //Debug.Log(v);
        //Debug.Log(new Vector2Int(v.x / 2, v.y / 2));
        return new Vector2Int(v.x / 2, v.y / 2);

    }
    public Vector2 arrayCellSize()
    {
        return 2 * tilemapReal.cellSize;
    }
    TileType parseMapTile(string s)
    {
        if(s == null)
        {
            return TileType.NULL;
        }
        int id = Int32.Parse(s.Replace("coloredTiles_", ""));
        switch (id)
        {
            case 0:
                return TileType.PLAIN;
            case 1:
                return TileType.FOREST;
            case 2:
                return TileType.MOUNTAIN;
            case 3:
                return TileType.FORT;
            case 4:
                return TileType.WALL;
            case 5:
                return TileType.IMPASSABLEWALL;
            case 6:
                return TileType.PEAK;
            default:
                return TileType.NULL;
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
        debugTiles.SetLine(arrayPosToWorld(arrayPosA) + (Vector2)tilemapReal.cellSize, arrayPosToWorld(arrayPosB) + (Vector2)tilemapReal.cellSize);
    }
    public void ClearDebugLines()
    {
        debugTiles.ClearLines();
    }

    public static WorldMap GetInstance()
    {
        return GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
    }
}

public enum TileType
{
    FOREST, MOUNTAIN, PLAIN, FORT, IMPASSABLEWALL, NULL, WALL, PEAK
}


public class MapTile
{
    

    public TileType tile;
    public GameObject modifier;

    public override string ToString()
    {
        return tile.ToString();
    }
    public Boolean GetPassable(MapUnit c)
    {
        if(tile == TileType.NULL)
        {
            return false;
        }
 
        if(c.movementType == MovementType.FLIER)
        {
            return tile != TileType.IMPASSABLEWALL;
        }
        return tile != TileType.IMPASSABLEWALL && tile != TileType.WALL && tile != TileType.PEAK;




    }
    public float GetCost(MapUnit c)
    {
        if(c.movementType == MovementType.FLIER)
        {
            return 1;
        }
        if (c.movementType == MovementType.GROUNDED)
        {
            switch (tile)
            {
                case TileType.FOREST:
                    return 2;
                case TileType.MOUNTAIN:
                    return 3;
                case TileType.PLAIN:
                    return 1;
                case TileType.FORT:
                    return 2;
                
                default:
                    return 1;
            }
        }
        if(c.movementType == MovementType.MOUNTED)
        {
            switch (tile)
            {
                case TileType.FOREST:
                    return 4;
                case TileType.MOUNTAIN:
                    return 6;
                case TileType.PLAIN:
                    return 1;
                case TileType.FORT:
                    return 3;
                default:
                    return 1;
            }
        }
        return 1;

    }

}

