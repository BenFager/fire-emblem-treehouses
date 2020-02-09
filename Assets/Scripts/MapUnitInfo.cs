using System;
using UnityEngine;

[Serializable]
public class MapUnitInfo
{
    public string id;
    public string name;
    public Sprite image;
    public InventoryItem weapon;
    public int maxHP;
    public int HP;
    public int speed;
    public int attack;
    public int hit;
    public int crit;
}
