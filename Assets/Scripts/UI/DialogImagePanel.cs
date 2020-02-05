using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogImagePanel : MonoBehaviour, IUIPanel
{
    Animator anim;
    Image image;
    void Awake()
    {
        anim = GetComponent<Animator>();
        image = transform.Find("Position").GetComponent<Image>();
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
    public void Show(Sprite spr)
    {
        image.sprite = spr;
        Show();
    }
    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
