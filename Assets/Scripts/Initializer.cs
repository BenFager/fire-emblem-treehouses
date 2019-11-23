using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>().Initialize();
        GameObject.FindGameObjectWithTag("Cursor").GetComponent<Cursor>().Initialize();
        GameObject.FindGameObjectWithTag("CombatController").GetComponent<PlayerController>().Initialize();
        GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>().Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
