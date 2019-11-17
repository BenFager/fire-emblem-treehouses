using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DebugTiles : MonoBehaviour
{
    Tilemap tilemap;
    public Tile debugTile;
    public GameObject debugLinePrefab;

    List<GameObject> lines = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLine(Vector2 start, Vector2 end)
    {
        DebugLine line = Instantiate(debugLinePrefab).GetComponent<DebugLine>();
        line.SetLine(start, end);
        lines.Add(line.gameObject);
    }
    public void ClearLines()
    {
        foreach (GameObject go in lines)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }
        lines.Clear();
    }

    public void SetTile(Vector2 pos, Color c)
    {
        Vector3Int v = tilemap.WorldToCell(pos);
        SetSubtile(v, c, debugTile);
        SetSubtile(v + new Vector3Int(1, 0, 0), c, debugTile);
        SetSubtile(v + new Vector3Int(0, 1, 0), c, debugTile);
        SetSubtile(v + new Vector3Int(1, 1, 0), c, debugTile);
    }
    public void ClearTile(Vector2 pos)
    {
        Vector3Int v = tilemap.WorldToCell(pos);
        SetSubtile(v, Color.white, null);
        SetSubtile(v + new Vector3Int(1, 0, 0), Color.white, null);
        SetSubtile(v + new Vector3Int(0, 1, 0), Color.white, null);
        SetSubtile(v + new Vector3Int(1, 1, 0), Color.white, null);
    }
    public void ClearTiles()
    {
        tilemap.ClearAllTiles();
    }
    private void SetSubtile(Vector3Int v, Color c, Tile t)
    {
        tilemap.SetTile(v, t);
        tilemap.SetTileFlags(v, TileFlags.None);
        tilemap.SetColor(v, c);
    }
}
