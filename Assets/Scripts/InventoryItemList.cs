using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemList : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    Dictionary<string, InventoryItem> itemsById = new Dictionary<string, InventoryItem>();

    public static InventoryItemList GetInstance()
    {
        return GameObject.FindGameObjectWithTag("InventoryItemList")?.GetComponent<InventoryItemList>();
    }

    void Awake()
    {
        foreach (InventoryItem item in items)
        {
            itemsById[item.id] = item;
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

    public InventoryItem GetItemById(string id)
    {
        return itemsById[id];
    }
}
