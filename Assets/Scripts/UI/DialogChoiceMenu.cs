using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogChoiceMenu : MonoBehaviour, IUIPanel
{
    [Serializable]
    public class Choice
    {
        public string text;
        public UnityEvent result;
        public Choice(string text, UnityEvent result)
        {
            this.text = text;
            this.result = result;
        }
    }
    public List<Choice> choices = new List<Choice>();
    public List<UITextPanel> buttons = new List<UITextPanel>();

    // menu is in any state of open. Dialog should block advancement while this is true.
    public bool Active { get; private set; }

    // menu is ready to take input (animation has finished and menu is fully open)
    private bool ready = false;

    // choice has been selected. Ignore future inputs until menu is shown again.
    private bool choiceSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        Active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used to set menu contents
    public void SetChoices(List<Choice> choices)
    {
        if (choices.Count > buttons.Count)
        {
            throw new DialogError($"Cannot display list of {choices.Count} choices when this menu only supports {buttons.Count} options.");
        }
        this.choices = choices;
    }

    Coroutine show;
    public void Show()
    {
        Active = true;
        for (int i = 0; i < choices.Count; i++)
        {
            buttons[i].Show(choices[i].text);
        }
        // reset choiceSelected
        choiceSelected = false;
        // become ready a little bit later when the menu is actually shown
        if (show != null)
        {
            StopCoroutine(show);
        }
        show = StartCoroutine(Activate(0.1f));
    }
    IEnumerator Activate(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
        show = null;
    }

    Coroutine hide;
    public void Hide()
    {
        ready = false;
        foreach (UITextPanel button in buttons)
        {
            button.Hide();
        }
        // deactivate a little bit later when the menu is actually hidden
        if (hide != null)
        {
            StopCoroutine(hide);
        }
        hide = StartCoroutine(Deactivate(0.1f));
    }
    IEnumerator Deactivate(float time)
    {
        yield return new WaitForSeconds(time);
        Active = false;
        hide = null;
    }

    // handles player clicking on one of the menu options
    public void SelectChoice(int id)
    {
        if (!ready || choiceSelected)
        {
            return;
        }
        if (id >= choices.Count)
        {
            throw new DialogError($"Menu option {id + 1} was chosen when there were only {choices.Count} options.");
        }
        choices[id].result.Invoke();
        Hide();
        DialogRunner.RequestAdvance();
        choiceSelected = true;
    }
}
