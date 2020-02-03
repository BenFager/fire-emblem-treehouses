using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogUI : MonoBehaviour, IDialogUI
{
    public void DisplayText(string name, string text)
    {
        if (name == null)
        {
            Debug.Log(text);
        }
        else
        {
            Debug.Log($"{name}: {text}");
        }
    }

    public void HideCharacter(string id)
    {
        Debug.Log($"Hide character {id}");
    }

    public void ShowCharacter(string id, string image, DialogImageOptions options)
    {
        Debug.Log($"Show character {id} with image {image} with options {options}");
    }

    public void Finish()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
