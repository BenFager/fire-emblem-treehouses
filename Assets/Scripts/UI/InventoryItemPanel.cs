using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour, IUIPanel
{
    Animator anim;
    Image image;
    Text text;
    Text info;
    Image frame;

    void Awake()
    {
        anim = GetComponent<Animator>();
        text = transform.Find("Position").Find("Text").GetComponent<Text>();
        info = transform.Find("Position").Find("TextItemInfo").GetComponent<Text>();
        image = transform.Find("Position").Find("Image").GetComponent<Image>();
        frame = transform.Find("Position").Find("Frame").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    // should always be shown since it is expensive to do the animation
    // fade in via parent canvas group instead
    public void Show()
    {
    }

    public void Hide()
    {
    }

    public void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            frame.color = new Color(50 / 255f, 11 / 255f, 148 / 255f);
        }
        else
        {
            frame.color = Color.black;
        }
    }
}
