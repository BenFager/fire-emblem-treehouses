using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour
{
    Animator anim;
    Image image;
    Text text;
    Text info;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        text = transform.Find("Position").Find("Text").GetComponent<Text>();
        info = transform.Find("Position").Find("TextItemInfo").GetComponent<Text>();
        image = transform.Find("Position").Find("Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(InventoryItem item)
    {
        text.text = item.name;
        info.text = "";
        if (item.maxUses > 0)
        {
            info.text = $"{item.uses} / {item.maxUses}";
        }
        image.sprite = item.sprite;
    }

    public void Show()
    {
        anim.SetBool("Active", true);
    }

    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
