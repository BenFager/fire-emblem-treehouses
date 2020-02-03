using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour, IDialogUI
{
    DialogImagePanel imagePanel;
    DialogTextPanel namePanel;
    DialogTextPanel textPanel;

    [Serializable]
    public struct DialogSprite
    {
        public string name;
        public Sprite sprite;
    }
    public List<DialogSprite> spriteList = new List<DialogSprite>();
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        Transform dialogCanvas = transform.Find("DialogCanvas");
        imagePanel = dialogCanvas.Find("ImagePanel").GetComponent<DialogImagePanel>();
        namePanel = dialogCanvas.Find("NamePanel").GetComponent<DialogTextPanel>();
        textPanel = dialogCanvas.Find("TextPanel").GetComponent<DialogTextPanel>();
        // get dialog sprites
        foreach (DialogSprite s in spriteList)
        {
            sprites[s.name] = s.sprite;
        }
        // load dialog and run
        DialogRunner.Run(this, "Assets/Dialog/control_test.yaml");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DialogRunner.RequestAdvance();
        }
    }

    public void DisplayText(string name, string text)
    {
        if (name != null)
        {
            namePanel.Show(name);
        }
        else
        {
            namePanel.Hide();
        }
        textPanel.Show(text);
    }

    public void ShowCharacter(string id, string image, DialogImageOptions options)
    {
        imagePanel.Show(sprites[image]);
        DialogRunner.RequestAdvance();
    }

    public void HideCharacter(string id)
    {
        imagePanel.Hide();
        DialogRunner.RequestAdvance();
    }

    public void Finish()
    {
        namePanel.Hide();
        textPanel.Hide();
        imagePanel.Hide();
    }
}
