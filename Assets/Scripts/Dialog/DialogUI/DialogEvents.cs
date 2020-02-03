using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogUtils;

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

    // Start is called before the first frame update
    void Start()
    {
        
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
            default:
                throw new DialogError($"Unknown event id '{args["id"]}'.\nYAML: {Reserialize(o)}");
        }
    }
}
