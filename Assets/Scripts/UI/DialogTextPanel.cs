using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTextPanel : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string text)
    {
        anim.SetBool("Active", true);
        this.text.text = text;
    }
    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
