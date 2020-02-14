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
    int index = 0;
    bool Active => anim.GetCurrentAnimatorStateInfo(0).IsName("Shown");

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.Find("Canvas").Find("InventoryPanel").GetComponent<Animator>();
        // StartCoroutine(OpenInventory());
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                index++;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                index--;
            }
            index = Mathf.Clamp(index, 0, itemListPanel.Count - 1);
            marker.MoveTo(index);
            itemListPanel.Highlight(index);
        }
    }

    public void Show()
    {
        index = 0;
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
