using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextPanel : MonoBehaviour, IUIPanel
{
    public bool startShown = false;

    Animator anim;
    Text text;
    void Awake()
    {
        anim = GetComponent<Animator>();
        text = transform.Find("Position").Find("Text").GetComponent<Text>();
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

    public void Show()
    {
        anim.SetBool("Active", true);
    }
    public void Show(string text)
    {
        this.text.text = text;
        Show();
    }
    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
