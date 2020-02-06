using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMarker : MonoBehaviour, IUIPanel
{
    RectTransform pos;
    UIImagePanel image;
    public InventoryItemListPanel itemsPanel;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<RectTransform>();
        image = GetComponent<UIImagePanel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTo(int i)
    {
        // scroll items to index
        itemsPanel.ScrollToPanel(i - 2);
        itemsPanel.ScrollToPanel(i);
        // get cursor position at index
        pos.anchoredPosition = new Vector2(pos.anchoredPosition.x, itemsPanel.GetPanelPos(i));
    }

    public void Show()
    {
        image.Show();
    }
    public void Hide()
    {
        image.Hide();
    }
}
