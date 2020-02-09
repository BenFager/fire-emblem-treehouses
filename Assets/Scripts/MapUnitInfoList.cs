using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnitInfoList : MonoBehaviour
{
    public List<MapUnitInfo> units = new List<MapUnitInfo>();
    Dictionary<string, MapUnitInfo> itemsById = new Dictionary<string, MapUnitInfo>();

    public static MapUnitInfoList GetInstance()
    {
        return GameObject.FindGameObjectWithTag("MapUnitInfoList")?.GetComponent<MapUnitInfoList>();
    }

    void Awake()
    {
        foreach (MapUnitInfo unit in units)
        {
            itemsById[unit.id] = unit;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public MapUnitInfo GetUnitById(string id)
    {
        return itemsById[id];
    }
}
