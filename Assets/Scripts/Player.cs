using System;
using System.Collections;
using System.Collections.Generic;
using SaintsField;
using UnityEngine;

public class Player : MonoBehaviour,  IEventAggregator
{
    [AboveButton(nameof(TestAttackOne))]
    [SerializeField] private float hp;
    [SerializeField] private float hpMax;

    public float Hp => hp;
    public float HpMax => hpMax;

    public string ID => nameof(Player);
    public EventType Type => EventType.Player;

    void Start()
    {
        // eventManager.聽("攻擊玩家", "多少");
        Func<(float hp, float hpMax)> func = OnHpChanged;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.HpChanged, func);
    }

    private (float, float) OnHpChanged()
    {
        return (hp, hpMax);
    }

    private void OnPlayerAttacked(float attackPower)
    {
        float oldHp = hp;
        hp -= attackPower;
        if (hp < 0)
            hp = 0;
        if (hp != oldHp)
        {
            // eventManager.Raise("玩家血改變了", oldHp, newHp);
            EventAggregator.Instance.ManualTrigger(("Player", EventType.Player, EventBehaviorType.HpChanged));
        }
    }

    private void TestAttackOne()
    {
        OnPlayerAttacked(1);
    }
}
