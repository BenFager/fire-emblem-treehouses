using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextPanel : MonoBehaviour, IUIPanel
{
    public bool startShown = false;

    Animator anim;
    Text text;
    Image frame;
    void Awake()
    {
        anim = GetComponent<Animator>();
        text = transform.Find("Position").Find("Text").GetComponent<Text>();
        frame = transform.Find("Position").Find("Frame").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startShown)
        {
            Show();
        }
    }

    // Update is called once per frame
    void Update()
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
    public void Set(string text)
    {
        this.text.text = text;
    }
    public void Show()
    {
        if (anim != null)
        {
            anim.SetBool("Active", true);
        }
    }
    public void Show(string text)
    {
        Set(text);
        Show();
    }
    public void Hide()
    {
        if (anim != null)
        {
            anim.SetBool("Active", false);
        }
    }
}
