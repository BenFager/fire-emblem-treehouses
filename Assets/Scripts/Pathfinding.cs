﻿using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    WorldMap worldMap;
    AStarNode[,] aStarNodeArray;
    
    
    static List<Vector2Int> neighborCells = new List<Vector2Int> {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
    };

    // Start is called before the first frame update
    void Start()
    {
        worldMap = GetComponent<WorldMap>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float Distance(Vector2Int start, Vector2Int goal)
    {
        return (Mathf.Pow(Mathf.Pow((start.x - goal.x), 2) + Mathf.Pow(start.y - goal.y, 2), .5f));
    }

    private float Cost(MapTile[,] mapTiles, Vector2Int start, Vector2Int neighbor, MapUnit c)
    {
        return (mapTiles[neighbor.x, neighbor.y].GetCost(c));
    }

    private List<Vector2Int> GetNeighbors(AStarNode current, MapTile[,] mapTiles, MapUnit c)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        foreach (Vector2Int v in neighborCells)
        {
            Vector2Int newPos = current.pos + v;
            if (newPos.x < 0 || newPos.y < 0)
            {
                continue;
            }
            if(newPos.x >= mapTiles.GetLength(0) || newPos.y >= mapTiles.GetLength(1))
            {
                continue;
            }
            if(!mapTiles[newPos.x, newPos.y].GetPassable(c))
            {
                continue;
            }
            neighbors.Add(newPos);
        }
        return neighbors;
    }

    public List<PathNode> Pathfind(MapTile[,] mapTiles, Vector2Int start, Vector2Int goal, MapUnit c)
    {
        return AStar(mapTiles, start, goal, -1, true, c);
    }
    public List<PathNode> GetPaths(MapTile[,] mapTiles, Vector2Int start, float maxCost, MapUnit c)
    {
        return AStar(mapTiles, start, new Vector2Int(-1, -1), maxCost, false, c);
    }
    public List<PathNode> GetAIPath(MapTile[,] mapTiles, Vector2Int start, Vector2Int goal, float maxCost, MapUnit c)
    {
        return AStar(mapTiles, start, goal, maxCost, true, c);
    }
    //public int getCostBetweenPoints(Vector2Int pointOne, Vector2Int pointTwo)
    //{

    //}


    private List<PathNode> AStar(MapTile[,] mapTiles, Vector2Int start, Vector2Int goal, float maxCost, bool pathMode, MapUnit c)//True: return path, False, return all possibilities
    {
        List<PathNode> nodesInRange = new List<PathNode>();
        aStarNodeArray = new AStarNode[mapTiles.GetLength(0), mapTiles.GetLength(1)];
        for(int x = 0; x < mapTiles.GetLength(0); x++)
        {
            for(int y = 0; y < mapTiles.GetLength(1); y++)
            {
                aStarNodeArray[x, y] = new AStarNode(new Vector2Int(x, y));
                aStarNodeArray[x, y].gScore = float.PositiveInfinity;
                if(pathMode)
                {
                    aStarNodeArray[x, y].hScore = Distance(new Vector2Int(x, y), goal);
                }
                else
                {
                    aStarNodeArray[x, y].hScore = 0;
                }
                
            }
        }
        aStarNodeArray[start.x, start.y].gScore = 0;
        FastPriorityQueue<AStarNode> openSet = new FastPriorityQueue<AStarNode>(aStarNodeArray.GetLength(0) * aStarNodeArray.GetLength(1));

        openSet.Enqueue(aStarNodeArray[start.x, start.y], aStarNodeArray[start.x, start.y].fScore);



        //worldMap.SetDebugTile(start, Color.cyan);
        //worldMap.SetDebugTile(start, Color.red);
        if (pathMode && (!mapTiles[start.x, start.y].GetPassable(c) || !mapTiles[goal.x, goal.y].GetPassable(c) ))
        {
            return ReconstructPath(start, goal, aStarNodeArray, maxCost);
        }
        while (openSet.Count != 0)
        {
            AStarNode currentNode = openSet.Dequeue();

            if (currentNode.gScore > maxCost && !pathMode)
            {
                return nodesInRange;
            }

            if (pathMode && currentNode.pos == goal)
            {
                //worldMap.SetDebugTile(goal, Color.green);
                return ReconstructPath(start, goal, aStarNodeArray, maxCost);
            }
            currentNode.closed = true;
           // worldMap.SetDebugTile(currentNode.pos, Color.blue);
            if(!pathMode)
            {
                nodesInRange.Add(new PathNode(currentNode.pos, currentNode.gScore));
            }
            foreach (Vector2Int v in GetNeighbors(currentNode, mapTiles, c))
            {
                AStarNode currentNeighbor = aStarNodeArray[v.x, v.y];
                if (currentNeighbor.closed)
                {
                    continue;
                }
                if (!openSet.Contains(currentNeighbor))
                {
                    openSet.Enqueue(currentNeighbor, currentNeighbor.fScore);
                    //worldMap.SetDebugTile(currentNeighbor.pos, Color.magenta);
                    currentNeighbor.prevPos = currentNode.pos;
                }
                float tentative_gScore = currentNode.gScore + Cost(mapTiles, currentNode.pos, currentNeighbor.pos, c);
                if (tentative_gScore < currentNeighbor.gScore)
                {
                    // This path to neighbor is better than any previous one. Record it!
                    currentNeighbor.prevPos = currentNode.pos;
                    currentNeighbor.gScore = tentative_gScore;
                    openSet.UpdatePriority(currentNeighbor, currentNeighbor.fScore);
                }
            }

        }
        if (pathMode)
        {
            return ReconstructPath(start, goal, aStarNodeArray, maxCost);
        }
        else
        {
            return nodesInRange;
        }
    }


    private List<PathNode> ReconstructPath(Vector2Int start, Vector2Int goal, AStarNode[,] nodes, float maxCost)
    {
        List<PathNode> path = new List<PathNode>();
        AStarNode current = nodes[goal.x, goal.y];

        if ((maxCost > 0 && current.gScore <= maxCost) || (maxCost < 0))
        {
            path.Add(new PathNode(goal, current.gScore));
        }
        
        while (current.prevPos != new Vector2Int(-1, -1) && path.Count < nodes.GetLength(0) * nodes.GetLength(1))
        {
            current = nodes[current.prevPos.x, current.prevPos.y];
            if ((maxCost > 0 && current.gScore <= maxCost) || (maxCost < 0))
            {
                path.Add(new PathNode(current.pos, current.gScore));
            }
            
        }
        if(current.pos != start)
        {
            return null;
        }

        path.Reverse();
        return path;
    }
    
}



class AStarNode : FastPriorityQueueNode
{
    public Vector2Int pos;
    public Vector2Int prevPos = new Vector2Int(-1, -1);
    public bool closed;
    public float gScore; //Cost so far
    public float hScore; //Uses Heuristics to tell which nodes to use first
    public float fScore { get { return gScore + hScore; } }
    public AStarNode(Vector2Int pos)
    {
        this.pos = pos;
    }
}

public struct PathNode
{
    public Vector2Int pos;
    public float cost;
    public PathNode(Vector2Int pos, float cost)
    {
        this.pos = pos;
        this.cost = cost;
    }
}