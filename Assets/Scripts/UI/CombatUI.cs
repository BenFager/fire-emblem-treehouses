using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour, IUIPanel
{
    public List<GameObject> actionPanels;
    public CombatUIMarker marker;
    Animator anim;
    int index = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Set(new List<string>()
        {
            "Attack", "Ability", "Item", "Trade", "Wait"
        });
        marker.MoveTo(0);
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        // test combat menu
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
        }
        index = Mathf.Clamp(index, 0, actionPanels.Count - 1);
        marker.MoveTo(index);
        Highlight(index);
    }

    public float GetPanelPos(int i)
    {
        return actionPanels[i].GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void Set(List<string> actions)
    {
        if (actions.Count == 0)
        {
            actionPanels[0].SetActive(false);
            return;
        }
        actionPanels[0].SetActive(true);
        // add action panels until we reach the number of actions
        for (int i = actionPanels.Count; i < actions.Count; i++)
        {
            GameObject newPanel = Instantiate(actionPanels[0], actionPanels[0].transform.parent);
            actionPanels.Add(newPanel);
        }
        // remove action panels until we reach the number of actions
        for (int i = actions.Count; i < actionPanels.Count; i++)
        {
            Destroy(actionPanels[i]);
            actionPanels.RemoveAt(i);
            i--;
        }
        // set action panel text
        for (int i = 0; i < actions.Count; i++)
        {
            actionPanels[i].GetComponent<UITextPanel>().Set(actions[i]);
        }
    }

    // highlights a panel as selection
    public void Highlight(int i)
    {
        foreach (GameObject p in actionPanels)
        {
            p.GetComponent<UITextPanel>().SetHighlight(false);
        }
        actionPanels[i].GetComponent<UITextPanel>().SetHighlight(true);
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
