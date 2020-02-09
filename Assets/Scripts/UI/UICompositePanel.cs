using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICompositePanel : MonoBehaviour, IUIPanel
{
    public List<GameObject> subpanels = new List<GameObject>();
    List<IUIPanel> castedSubpanels = new List<IUIPanel>();
    void Awake()
    {
        foreach (GameObject subpanel in subpanels)
        {
            IUIPanel panel = subpanel.GetComponents<IUIPanel>().LastOrDefault();
            if (panel == null)
            {
                throw new DialogError($"GameObject {subpanel.name} does not contain a component that implements IUIPanel.");
            }
            castedSubpanels.Add(panel);
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

    public void Show()
    {
        foreach (IUIPanel subpanel in castedSubpanels)
        {
            subpanel.Show();
        }
    }
    public void Hide()
    {
        foreach (IUIPanel subpanel in castedSubpanels)
        {
            subpanel.Hide();
        }
    }
}
