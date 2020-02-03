using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogRunner.Run(GetComponent<TestDialogUI>(), "Assets/Dialog/control_test.yaml");
    }

    // Update is called once per frame
    void Update()
    {
        DialogRunner.RequestAdvance();
    }
}
