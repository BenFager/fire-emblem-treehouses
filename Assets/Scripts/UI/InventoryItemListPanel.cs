using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemListPanel : MonoBehaviour, IUIPanel
{
    public List<InventoryItemPanel> itemPanels = new List<InventoryItemPanel>();
    public int maxPanels = 20;
    public float scrollPanelSize = 800;
    public float itemPanelsOffset = -115;

    List<InventoryItem> items = new List<InventoryItem>();

    // ui elements
    UITextPanel basePanel;
    RectTransform scrollPanel;
    Animator itemPanelBox;

    // used for calculating item panel position
    RectTransform firstPanel;
    RectTransform lastPanel;

    void Awake()
    {
        basePanel = GetComponent<UITextPanel>();
        scrollPanel = transform.Find("ScrollBox").Find("ScrollBoxContent").GetComponent<RectTransform>();
        itemPanelBox = transform.Find("ScrollBox").GetComponent<Animator>();
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
        for (int i = itemPanels.Count - 2; i < items.Count - 2; i++)
        {
            if (i < 0)
            {
                throw new DialogError($"Cannot add panels with only 1 element. At least 2 panels must be in the itemPanels list.");
            }
            GameObject nextPanel = Instantiate(itemPanels[i + 1].gameObject, itemPanels[i + 1].transform.parent);
            nextPanel.name = $"ItemPanel{i + 3}";
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
        // set position markers
        firstPanel = itemPanels.First().GetComponent<RectTransform>();
        lastPanel = itemPanels.Last().GetComponent<RectTransform>();
    }

    public void Show()
    {
        basePanel.Show();
        itemPanelBox.SetBool("Active", true);
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

    public void Hide()
    {
        basePanel.Hide();
        itemPanelBox.SetBool("Active", false);
    }

    /// used by InventoryMarker
    // local position of each panel's upper edge y position
    private float GetPanelRawPos(int i)
    {
        return Mathf.LerpUnclamped(firstPanel.anchoredPosition.y, lastPanel.anchoredPosition.y, (float)i / (itemPanels.Count - 1));
    }
    // gets the position of an item panel relative to the parent frame
    // used to place the cursor pointing to an item
    public float GetPanelPos(int i)
    {
        // take local y position, convert to parent frame y position
        return GetPanelRawPos(i) + scrollPanel.anchoredPosition.y + itemPanelsOffset + scrollPanel.sizeDelta.y;
    }
    // Scroll panel to fit a certain item panel.
    public void ScrollToPanel(int i)
    {
        float posY = scrollPanel.anchoredPosition.y;
        float pos = GetPanelPos(i);
        // if target panel is out of bounds, place it in bounds
        if (pos > itemPanelsOffset)
        {
            posY += itemPanelsOffset - pos;
        }
        if (pos < itemPanelsOffset - scrollPanelSize)
        {
            posY += (itemPanelsOffset - scrollPanelSize) - pos;
        }
        scrollPanel.anchoredPosition = new Vector2(scrollPanel.anchoredPosition.x, Mathf.Clamp(posY, -scrollPanel.sizeDelta.y, 0));
    }

    // highlights a panel as selection
    public void Highlight(int i)
    {
        foreach (InventoryItemPanel p in itemPanels)
        {
            p.SetHighlight(false);
        }
        itemPanels[i].SetHighlight(true);
    }
}
