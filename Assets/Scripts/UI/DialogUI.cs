using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour, IDialogUI
{
    UIImagePanel imagePanel;
    UITextPanel namePanel;
    UITextPanel textPanel;

    [Serializable]
    public struct DialogSprite
    {
        public string name;
        public Sprite sprite;
    }
    public List<DialogSprite> spriteList = new List<DialogSprite>();
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    // text scroll animation
    Coroutine textScroll;

    // dialog choice menu
    DialogChoiceMenu menu;

    // Start is called before the first frame update
    void Start()
    {
        Transform dialogCanvas = transform.Find("Canvas");
        imagePanel = dialogCanvas.Find("ImagePanel").GetComponent<UIImagePanel>();
        namePanel = dialogCanvas.Find("NamePanel").GetComponent<UITextPanel>();
        textPanel = dialogCanvas.Find("TextPanel").GetComponent<UITextPanel>();
        menu = GameObject.FindGameObjectWithTag("DialogChoiceMenu").GetComponent<DialogChoiceMenu>();
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
        if (Input.GetMouseButtonDown(0) && textScroll == null && !menu.Active)
        {
            DialogRunner.RequestAdvance();
        }
    }

    public void DisplayText(string name, string text)
    {
        // set name panel
        if (name != null)
        {
            namePanel.Show(name);
        }
        else
        {
            namePanel.Hide();
        }
        // stop old text scroll animation if it is still running
        if (textScroll != null)
        {
            StopCoroutine(textScroll);
            textScroll = null;
        }
        // use speed as Mathf.Infinity to turn off scrolling
        textScroll = StartCoroutine(ScrollText(text, 75f));
    }
    // speed is in characters/second
    IEnumerator ScrollText(string text, float speed)
    {
        float timer = Time.deltaTime;
        int len = 0;
        // loop until we reach end of text
        do
        {
            timer += Time.deltaTime;
            len = (int)Math.Min(text.Length, speed * timer);
            yield return null;
            // clicking forwards text scroll to end
            if (Input.GetMouseButtonDown(0))
            {
                len = text.Length;
                yield return null;
            }
            textPanel.Show(text.Substring(0, len));
        }
        while (len < text.Length);
        // finish
        textScroll = null;
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
        if (textScroll != null)
        {
            StopCoroutine(textScroll);
            textScroll = null;
        }
        imagePanel.Hide();
    }
}
