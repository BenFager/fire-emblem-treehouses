using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour, IUIPanel
{
    public UICompositePanel inventoryPanel;
    public InventoryItemListPanel itemListPanel;
    public InventoryMarker marker;
    Animator anim;
    bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.Find("Canvas").Find("InventoryPanel").GetComponent<Animator>();
        StartCoroutine(OpenInventory());
    }
    IEnumerator OpenInventory()
    {
        yield return new WaitForSeconds(2f);
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
        Show();
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
        p = Mathf.Clamp(p, 0, itemListPanel.Count - 1);
        marker.MoveTo(p);
        itemListPanel.Highlight(p);
    }

    public void Show()
    {
        if (anim != null)
        {
            anim.SetBool("Active", true);
        }
    }

    public void Hide()
    {
        if (anim != null)
        {
            anim.SetBool("Active", false);
        }
    }
}
