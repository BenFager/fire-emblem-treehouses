using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatUIActions : MonoBehaviour
{
    [Serializable]
    public class Action
    {
        public string id;
        public string name;
        public UnityEvent action;
    }
    public List<Action> actions;
    Dictionary<string, Action> actionsById = new Dictionary<string, Action>();

    void Awake()
    {
        foreach (Action action in actions)
        {
            actionsById[action.id] = action;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Action GetActionById(string id)
    {
        return actionsById[id];
    }

    // causes a panel to blink. Useful when changing targets
    // only intended to be called from another coroutine
    IEnumerator Blink(IUIPanel panel)
    {
        panel.Hide();
        yield return new WaitForSeconds(0.25f);
        panel.Show();
    }
}
