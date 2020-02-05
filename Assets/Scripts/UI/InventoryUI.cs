using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    IUIPanel inventoryPanel;
    InventoryItemListPanel itemListPanel;

    // Start is called before the first frame update
    void Start()
    {
        Transform invPanel = transform.Find("Canvas").Find("InventoryPanel");
        inventoryPanel = invPanel.GetComponent<IUIPanel>();
        itemListPanel = invPanel.Find("Position").Find("ItemListPanel").GetComponent<InventoryItemListPanel>();
        // test UI
        List<InventoryItem> testItems = new List<InventoryItem>();
        InventoryItemList itemList = InventoryItemList.GetInstance();
        for (int i = 0; i < 6; i++)
        {
            testItems.Add(itemList.GetItemById("sword"));
            testItems.Add(itemList.GetItemById("axe"));
            testItems.Add(itemList.GetItemById("spear"));
        }
        itemListPanel.SetItems(testItems);
        inventoryPanel.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
