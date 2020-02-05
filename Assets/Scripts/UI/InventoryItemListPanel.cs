using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemListPanel : MonoBehaviour, IUIPanel
{
    public List<InventoryItemPanel> itemPanels = new List<InventoryItemPanel>();
    public int maxPanels = 20;
    public float scrollPanelSize = 1015;

    List<InventoryItem> items = new List<InventoryItem>();

    // ui elements
    UITextPanel basePanel;
    RectTransform scrollPanel;

    void Awake()
    {
        basePanel = GetComponent<UITextPanel>();
        scrollPanel = transform.Find("ScrollBox").Find("ScrollBoxContent").GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Items to display in the UI.
    // Use before Show()
    public void SetItems(List<InventoryItem> items)
    {
        if (items.Count > maxPanels)
        {
            throw new DialogError($"Cannot display list of {items.Count} items when this menu only supports {maxPanels} items.");
        }
        this.items = items;
        // add panels as necessary
        for (int i = itemPanels.Count - 2; i < items.Count; i++)
        {
            if (i < 0)
            {
                throw new DialogError($"Cannot add panels with only 1 element. At least 2 panels must be in the itemPanels list.");
            }
            GameObject nextPanel = Instantiate(itemPanels[i + 1].gameObject, itemPanels[i + 1].transform.parent);
            nextPanel.name = $"ItemPanel{i + 1}";
            itemPanels.Add(nextPanel.GetComponent<InventoryItemPanel>());
            RectTransform nextPanelRect = nextPanel.GetComponent<RectTransform>();
            RectTransform lastPanelRect = itemPanels[i + 1].GetComponent<RectTransform>();
            RectTransform prevPanelRect = itemPanels[i].GetComponent<RectTransform>();
            // position of next panel is lastPanel.position + (lastPanel.position - prevPanel.position)
            nextPanelRect.anchoredPosition = (2 * lastPanelRect.anchoredPosition) - prevPanelRect.anchoredPosition;
        }
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
        // check if we need to display scrollbar
        scrollPanel.sizeDelta = Vector2.zero;
        if (itemPanels.Count > 0)
        {
            RectTransform last = itemPanels.Last().GetComponent<RectTransform>();
            float vertSize = Mathf.Abs(last.anchoredPosition.y + last.sizeDelta.y);
            if (vertSize > scrollPanelSize)
            {
                scrollPanel.sizeDelta = new Vector2(scrollPanel.sizeDelta.x, vertSize - scrollPanelSize);
            }
        }
    }
    // coroutine to handle scrolling and UI marker

    public void Hide()
    {
        basePanel.Hide();
        foreach (InventoryItemPanel itemPanel in itemPanels)
        {
            Hide();
        }
    }
}
