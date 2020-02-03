using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogImagePanel : MonoBehaviour
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

    public void Show(Sprite spr)
    {
        anim.SetBool("Active", true);
        image.sprite = spr;
    }
    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
