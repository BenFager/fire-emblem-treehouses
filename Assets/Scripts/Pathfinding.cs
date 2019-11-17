using Priority_Queue;
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
    Coroutine visualization;
    public bool complete = false;
    public List<Vector2Int> path;

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
    private float Cost(MapTile[,] mapTiles, Vector2Int start, Vector2Int neighbor)
    {
        return (mapTiles[neighbor.x, neighbor.y].GetCost());
    }
    private List<Vector2Int> GetNeighbors(AStarNode current, MapTile[,] mapTiles)
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
            if(!mapTiles[newPos.x, newPos.y].GetPassable())
            {
                continue;
            }
            neighbors.Add(newPos);
        }
        return neighbors;
    }

    public void AStar(MapTile[,] mapTiles, Vector2Int start, Vector2Int goal)
    {
        aStarNodeArray = new AStarNode[mapTiles.GetLength(0), mapTiles.GetLength(1)];
        for(int x = 0; x < mapTiles.GetLength(0); x++)
        {
            for(int y = 0; y < mapTiles.GetLength(1); y++)
            {
                aStarNodeArray[x, y] = new AStarNode(new Vector2Int(x, y));
                aStarNodeArray[x, y].gScore = float.PositiveInfinity;
                aStarNodeArray[x, y].hScore = /*Distance(new Vector2Int(x, y), goal)*/0;
            }
        }
        aStarNodeArray[start.x, start.y].gScore = 0;
        FastPriorityQueue<AStarNode> openSet = new FastPriorityQueue<AStarNode>(aStarNodeArray.GetLength(0) * aStarNodeArray.GetLength(1));

        openSet.Enqueue(aStarNodeArray[start.x, start.y], aStarNodeArray[start.x, start.y].fScore);

        complete = false;

        if (visualization != null)
        {
            StopCoroutine(visualization);
        }
        visualization = StartCoroutine(RunAStar(mapTiles, start, goal, aStarNodeArray, openSet));
    }

    private IEnumerator RunAStar(MapTile[,] mapTiles, Vector2Int start, Vector2Int goal, AStarNode[,] nodes, FastPriorityQueue<AStarNode> openSet)
    {
        worldMap.SetDebugTile(start, Color.cyan);
        worldMap.SetDebugTile(start, Color.red);
        if(!mapTiles[start.x, start.y].GetPassable() || !mapTiles[goal.x,goal.y].GetPassable())
        {
            complete = true;
            path = ReconstructPath(start, goal, nodes);
            yield break;
        }
        while (openSet.Count != 0)
        {
            AStarNode currentNode = openSet.Dequeue();

            if(currentNode.gScore > 5)
            {
                complete = true;
                path = ReconstructPath(start, goal, nodes);
                yield break;
            }

            if (currentNode.pos == goal)
            {
                worldMap.SetDebugTile(goal, Color.green);
                complete = true;
                path = ReconstructPath(start, goal, nodes);
                yield break;
            }
            currentNode.closed = true;
            worldMap.SetDebugTile(currentNode.pos, Color.blue);
            foreach (Vector2Int v in GetNeighbors(currentNode, mapTiles))
            {
                AStarNode currentNeighbor = nodes[v.x, v.y];
                if (currentNeighbor.closed)
                {
                    continue;
                }
                if (!openSet.Contains(currentNeighbor))
                {
                    openSet.Enqueue(currentNeighbor, currentNeighbor.fScore);
                    worldMap.SetDebugTile(currentNeighbor.pos, Color.magenta);
                    currentNeighbor.prevPos = currentNode.pos;
                }
                float tentative_gScore = currentNode.gScore + Cost(mapTiles, currentNode.pos, currentNeighbor.pos);
                if (tentative_gScore < currentNeighbor.gScore)
                {
                    // This path to neighbor is better than any previous one. Record it!
                    currentNeighbor.prevPos = currentNode.pos;
                    currentNeighbor.gScore = tentative_gScore;
                    openSet.UpdatePriority(currentNeighbor, currentNeighbor.fScore);
                }
            }
            yield return new WaitForSeconds(0.1f); 

        }
        complete = true;
        path = ReconstructPath(start, goal, nodes);
        yield break;
    }

    private List<Vector2Int> ReconstructPath(Vector2Int start, Vector2Int goal, AStarNode[,] nodes)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(goal);
        AStarNode current = nodes[goal.x, goal.y];
        while (current.prevPos != new Vector2Int(-1, -1) && path.Count < nodes.GetLength(0) * nodes.GetLength(1))
        {
            current = nodes[current.prevPos.x, current.prevPos.y];
            path.Add(current.pos);
        }
        if(current.pos != start)
        {
            return null;
        }
        else
        {
            path.Reverse();
            return path;
        }
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