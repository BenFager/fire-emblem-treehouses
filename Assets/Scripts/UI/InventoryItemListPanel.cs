using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemListPanel : MonoBehaviour
{
    public List<InventoryItemPanel> itemPanels = new List<InventoryItemPanel>();
    public int maxPanels = 5;
    public GameObject itemPanelPrefab;

    List<InventoryItem> items = new List<InventoryItem>();

    // ui elements
    UITextPanel basePanel;

    // Start is called before the first frame update
    void Start()
    {
        basePanel = GetComponent<UITextPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Items to display in the UI.
    // Use before Show()
    public void SetItems(List<InventoryItem> items)
    {
        if (items.Count > itemPanels.Count)
        {
            throw new DialogError($"Cannot display list of {items.Count} items when this menu only supports {maxPanels} items.");
        }
        this.items = items;
        // add panels as necessary
        
        // update items
        for (int i = 0; i < items.Count; i++)
        {
            itemPanels[i].SetItem(items[i]);
        }
    }

    public void Show()
    {
        basePanel.Show();
        for (int i = 0; i < items.Count; i++)
        {
            itemPanels[i].Show();
        }
    }

    public void Hide()
    {
        basePanel.Hide();
        foreach (InventoryItemPanel itemPanel in itemPanels)
        {
            Hide();
        }
    }
}
