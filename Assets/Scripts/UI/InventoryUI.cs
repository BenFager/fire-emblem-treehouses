using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public UICompositePanel inventoryPanel;
    public InventoryItemListPanel itemListPanel;
    public InventoryMarker marker;
    bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenInventory());
    }
    IEnumerator OpenInventory()
    {
        yield return new WaitForSeconds(5f);
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
        marker.Show();
        ready = true;
    }

    // Update is called once per frame
    static int p;
    void Update()
    {
        if (!ready)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            p++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            p--;
        }
        p = Mathf.Clamp(p, 0, 17);
        marker.MoveTo(p);
        itemListPanel.Highlight(p);
    }
}
