using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DialogUtils;
using static DialogChoiceMenu;

// dialog events are sections of code that can be
// run by the dialog system using:
//
// - event:
//     id: introDisplay
//     other_arg_here: value
public class DialogEvents : MonoBehaviour
{
    public static DialogEvents GetInstance()
    {
        return GameObject.FindGameObjectWithTag("DialogEvents")?.GetComponent<DialogEvents>();
    }

    DialogChoiceMenu menu;
    Dictionary<string, bool> flags;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("DialogChoiceMenu").GetComponent<DialogChoiceMenu>();
        flags = DialogRunner.GetInstance().Flags;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // put logic for dialog events here.
    public void HandleEvent(object o)
    {
        Dictionary<string, object> args = GetStatementArgs(o, "event");
        // invalid event
        if (!args.ContainsKey("id"))
        {
            throw new DialogError($"Event has no 'id' field.\nYAML: {Reserialize(o)}");
        }
        // handle events
        switch(args["id"])
        {
            // Prints out to the Unity debug console.
            // Usage:
            // - event:
            //     id: print
            //     value: some_text_here
            case "print":
                if (args.ContainsKey("value"))
                {
                    Debug.Log(args["value"]);
                }
                break;
            // Creates an example dialog branch menu.
            // Usage:
            // - event:
            //     id: test_menu
            case "test_menu":
                // reset flags
                flags["test_menu_choice_A"] = false;
                flags["test_menu_choice_B"] = false;
                flags["test_menu_choice_C"] = false;
                // create menu
                List<Choice> choices = new List<Choice>();
                choices.Add(new Choice("This chooses choice A.", new UnityEvent()));
                choices.Add(new Choice("This chooses choice B.", new UnityEvent()));
                choices.Add(new Choice("This chooses choice C.", new UnityEvent()));
                choices[0].result.AddListener(TestDialogMenuChoiceA);
                choices[1].result.AddListener(TestDialogMenuChoiceB);
                choices[2].result.AddListener(TestDialogMenuChoiceC);
                // open menu
                menu.SetChoices(choices);
                menu.Show();
                break;
            // unknown event
            default:
                throw new DialogError($"Unknown event id '{args["id"]}'.\nYAML: {Reserialize(o)}");
        }
    }

    // used for test_menu
    void TestDialogMenuChoiceA()
    {
        Debug.Log("Choice A was clicked!");
        flags["test_menu_choice_A"] = true;
    }
    void TestDialogMenuChoiceB()
    {
        Debug.Log("Choice B was clicked!");
        flags["test_menu_choice_B"] = true;
    }
    void TestDialogMenuChoiceC()
    {
        Debug.Log("Choice C was clicked!");
        flags["test_menu_choice_C"] = true;
    }
}
