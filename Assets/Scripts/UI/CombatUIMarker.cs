using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIMarker : MonoBehaviour, IUIPanel
{
    RectTransform pos;
    UIImagePanel image;
    public CombatUI combatUI;

    void Awake()
    {
        pos = GetComponent<RectTransform>();
        image = GetComponent<UIImagePanel>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTo(int i)
    {
        // move cursor position to panel
        pos.anchoredPosition = new Vector2(pos.anchoredPosition.x, combatUI.GetPanelPos(i));
    }

    public void Show()
    {
        image.Show();
    }
    public void Hide()
    {
        image.Hide();
    }
}
