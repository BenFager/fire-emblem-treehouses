using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string name;

    public int uses;

    // to disable, set to 0
    public int maxUses;

    public Sprite sprite;

    // TODO: add stats and stuff
}
