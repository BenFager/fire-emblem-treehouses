using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatInfoUI : MonoBehaviour, IUIPanel
{
    [Serializable]
    public class CombatInfoPanelTargets
    {
        public Text name;
        public InventoryItemPanel weapon;
        public Image image;
        public Text hpMax;
        public float hpBarSize;
        public RectTransform hpMarker;
        public Text hpMarkerText;
        public RectTransform hpRemaining;
        public RectTransform hpLost;
        public RectTransform hpLostDouble;
        public Text attack;
        public Text attackTwice;
        public Text hit;
        public Text crit;
    }
    public CombatInfoPanelTargets left;
    public CombatInfoPanelTargets right;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // test
        StartCoroutine(ShowCombatInfoUI());
    }
    IEnumerator ShowCombatInfoUI()
    {
        yield return new WaitForSeconds(3f);
        MapUnitInfoList units = MapUnitInfoList.GetInstance();
        Set(new CombatInfo(units.GetUnitById("ben"), units.GetUnitById("ben2")));
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // prepare menu for display, populate with data
    public void Set(CombatInfo info)
    {
        // left panel
        SetUnitInfo(left, info.left);
        SetCombatInfo(left, info.leftStats);
        SetHPBarInfo(left, info.left, info.rightStats);
        // right panel
        SetUnitInfo(right, info.right);
        SetCombatInfo(right, info.rightStats);
        SetHPBarInfo(right, info.right, info.leftStats);
    }
    private void SetUnitInfo(CombatInfoPanelTargets side, MapUnitInfo unit)
    {
        side.name.text = unit.name;
        side.weapon.SetItem(unit.weapon);
        side.hpMax.text = $"{unit.maxHP}";
    }
    private void SetCombatInfo(CombatInfoPanelTargets side, CombatInfo.CombatInfoStats stats)
    {
        side.attack.text = $"{stats.attack}";
        side.attackTwice.text = stats.attackTwice ? "x2" : "";
        side.hit.text = $"{stats.hit}";
        side.crit.text = $"{stats.crit}";
    }
    private void SetHPBarInfo(CombatInfoPanelTargets side, MapUnitInfo unit, CombatInfo.CombatInfoStats enemyStats)
    {
        int hpNoDouble = Math.Max(0, unit.HP - enemyStats.attack);
        int hpRemaining = Math.Max(0, hpNoDouble - (enemyStats.attackTwice ? enemyStats.attack : 0));
        SetHPBarWidth(unit.HP, hpNoDouble, unit.maxHP, side.hpLostDouble, side.hpBarSize);
        SetHPBarWidth(hpNoDouble, hpRemaining, unit.maxHP, side.hpLost, side.hpBarSize);
        SetHPBarWidth(hpRemaining, 0, unit.maxHP, side.hpRemaining, side.hpBarSize);
        // set HP marker. Left side goes further left (negative), right side goes right (positive)
        float hpMarkerPos = (side == left ? -1 : 1) * side.hpRemaining.sizeDelta.x;
        side.hpMarker.anchoredPosition = new Vector2(hpMarkerPos, side.hpMarker.anchoredPosition.y);
        side.hpMarkerText.text = $"{hpRemaining}";
    }
    private void SetHPBarWidth(int value1, int value2, int maxHP, RectTransform bar, float hpBarSize)
    {
        float width = Mathf.Lerp(0, hpBarSize, Mathf.Abs(value1 - value2) / (float)maxHP);
        bar.sizeDelta = new Vector2(width, bar.sizeDelta.y);
    }

    public void Show()
    {
        anim.SetBool("Active", true);
    }

    public void Hide()
    {
        anim.SetBool("Active", false);
    }
}
