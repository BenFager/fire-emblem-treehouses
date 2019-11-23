using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    public Vector2Int cameraPos;
    WorldMap worldMap;
    Cursor cursor;
    // Start is called before the first frame update
    void Start()

    {
        worldMap = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Rect camRect = CameraRect();
        Vector2 v = worldMap.arrayPosToWorld(cameraPos) + 0.5f * worldMap.arrayCellSize();
        Vector2 p = transform.position;
        p = Vector2.MoveTowards(p, v, 75 * Time.deltaTime);
        Vector2 min = worldMap.arrayPosToWorld((Vector2Int)worldMap.mapBounds.min) + 0.5f * camRect.size;
        Vector2 max = worldMap.arrayPosToWorld((Vector2Int)worldMap.mapBounds.max) - 0.5f * camRect.size;
        p = new Vector2(Mathf.Clamp(p.x, min.x, max.x), Mathf.Clamp(p.y, min.y, max.y));
        transform.position = new Vector3(p.x, p.y, transform.position.z);
        

        
        int camWidth = (int) (camRect.width / worldMap.arrayCellSize().x);
        int camHeight = (int)(camRect.height / worldMap.arrayCellSize().y);
        int scrollWidth = camWidth / 2 - 2;
        int scrollHeight = camHeight / 2 - 2;
        //if cursor.pos > rightward bound: shows how far right you need to go; if cursor.pos < leftward bound, it'll be negative, showing us how far left we need to go
        int dx = Math.Max(0, cursor.pos.x - (cameraPos.x + scrollWidth)) + Math.Min(0, cursor.pos.x - (cameraPos.x - scrollWidth));
        int dy = Math.Max(0, cursor.pos.y - (cameraPos.y + scrollHeight)) + Math.Min(0, cursor.pos.y - (cameraPos.y - scrollHeight));
        cameraPos += new Vector2Int(dx, dy);
    }

    public static Rect CameraRect()
    {
        Vector2 min = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        return new Rect(min, max - min);
    }
}
