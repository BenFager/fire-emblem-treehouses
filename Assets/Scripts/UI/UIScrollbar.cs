using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollbar : MonoBehaviour, IUIPanel
{
    Animator anim;
    Scrollbar scrollbar;
    void Awake()
    {
        anim = GetComponent<Animator>();
        scrollbar = transform.Find("Position").GetComponent<Scrollbar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        anim.SetBool("Active", true);
    }
    public void Set(float value)
    {
        scrollbar.value = value;
    }
    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
