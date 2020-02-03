using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public Coroutine Up;
    public Coroutine Down;
    public Coroutine Left;
    public Coroutine Right;
    public Vector2Int pos;
    public bool isEnabled = true;
    WorldMap worldMap;
    SpriteRenderer renderer;
    // Start is called before the first frame update
    public void Initialize()
    {
        worldMap = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        pos = new Vector2Int(15, 5);
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            Vector2 v = worldMap.arrayPosToWorld(pos) + 0.5f * worldMap.arrayCellSize();
            Vector2 p = transform.position;
            p = Vector2.MoveTowards(p, v, 20 * Time.deltaTime);
            transform.position = new Vector3(p.x, p.y, transform.position.z);

            HandleInput(KeyCode.UpArrow, ref Up, Vector2Int.up);
            HandleInput(KeyCode.DownArrow, ref Down, Vector2Int.down);
            HandleInput(KeyCode.RightArrow, ref Right, Vector2Int.right);
            HandleInput(KeyCode.LeftArrow, ref Left, Vector2Int.left);
        }
    }

    void HandleInput(KeyCode k, ref Coroutine c, Vector2Int v)
    {
        if(Input.GetKeyDown(k))
        {
            if(c != null)
            {
                StopCoroutine(c);
                
            }
            c = StartCoroutine(RapidKeyPress(k, v));
            
        }
        if(Input.GetKeyUp(k))
        {
            if(c != null)
            {
                StopCoroutine(c);
            }
        }
    }

    IEnumerator RapidKeyPress(KeyCode k, Vector2Int v)
    {
        movePos(pos, v);
        yield return new WaitForSeconds(0.25f);
        while (true)
        {
            movePos(pos, v);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void movePos(Vector2Int pos, Vector2Int v)
    {
        Vector2Int p = pos + v;
        if (p.x >= 0 && p.y >= 0 && p.x < worldMap.mapTiles.GetLength(0) && p.y < worldMap.mapTiles.GetLength(1))
        {
            if (worldMap.mapTiles[p.x, p.y].tile != TileType.NULL)
            {
                this.pos += v;
            }
        }
    }

    public void Enable()
    {
        isEnabled = true;
        renderer.color = Color.white;
    }
    public void DisableCursor()
    {
        isEnabled = false;
        renderer.color = Color.clear;
    }
}
