using System;

// Stores info on a hypothetical attack between two units.
// Left is attacker, right is defender.
// Used for the CombatInfoUI.
public class CombatInfo
{
    public MapUnitInfo left;
    public MapUnitInfo right;

    public CombatInfoStats leftStats;
    public CombatInfoStats rightStats;

    public class CombatInfoStats
    {
        public int attack;
        public bool attackTwice;
        public int hit;
        public int crit;
        public CombatInfoStats(int attack, bool attackTwice, int hit, int crit)
        {
            this.attack = attack;
            this.attackTwice = attackTwice;
            this.hit = hit;
            this.crit = crit;
        }
    }

    public CombatInfo(MapUnitInfo left, MapUnitInfo right)
    {
        // TODO: some processing to get atk, hit, crit, and double
        this.left = left;
        this.right = right;
        leftStats = new CombatInfoStats(left.attack, left.speed >= right.speed * 2, left.hit, left.crit);
        rightStats = new CombatInfoStats(right.attack, right.speed >= left.speed * 2, right.hit, right.crit);
    }
}