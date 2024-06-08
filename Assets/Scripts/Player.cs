using System;
using System.Collections;
using System.Collections.Generic;
using SaintsField;
using UnityEngine;
using UnityEngine.Events;

public interface IPlayer
{
    void Attacked(float attackPower);
    void AddHp(float value);
}

public class Player : MonoBehaviour, IEventAggregator, ITarget, IPlayer
{
    [AboveButton(nameof(TestAttack1))]
    [AboveButton(nameof(TestAttack10))]
    [AboveButton(nameof(TestAttack50))]
    [AboveButton(nameof(TestAttack200))]
    [SerializeField] private float hp;
    [SerializeField] private float hpMax;

    [Tooltip("多久 melt 一次")]
    [SerializeField] private float meltInterval = 3;
    [Tooltip("每一次 melt 傷害多少")]
    [SerializeField] private float meltDamage = 1;

    [SerializeField]
    private PlayerAnimController playerAnimController;

    // [SerializeField] private UnityEvent<float> onHpDecrease;
    [SerializeField] private UnityEvent onHpBecomeZero;

    private float nextMeltTime;

    public float Hp => hp;
    public float HpMax => hpMax;

    public string ID => nameof(Player);
    public EventType Type => EventType.Player;

    public Transform Trans => transform;

    public Animator Animator => playerAnimController.GetAnimator();

    void Start()
    {
        // eventManager.聽("攻擊玩家", "多少");
        Func<(float hp, float hpMax)> func = OnHpChanged;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.HpChanged, func);
        Func<ITarget> seekTarget = () => { return this; };
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.GetSeekTarget, seekTarget);
        Action<float> attack = Attacked;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.EnemyAttack, attack);

        nextMeltTime = Time.time + meltInterval;

        playerAnimController = GetComponent<PlayerAnimController>();
    }


    void Update()
    {
        if (hp <= 0)
        {
            return;
        }

        if (Time.time >= nextMeltTime)
        {
            nextMeltTime = Time.time + meltInterval;
            if (hp > 0)
            {
                OnPlayerAttacked(meltDamage); // 主角自己就會一直扣血
            }
        }

        var hpRatio = hp / hpMax;
        playerAnimController.UpdateMeltingAnim(hpRatio);
    }

    public void Attacked(float attackPower)
    {
        OnPlayerAttacked(attackPower);
    }

    private (float hp, float hpMax) OnHpChanged()
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
            if (hp == 0)
            {
                // 讓冰淇淋融化，或其他血量歸零演出
                onHpBecomeZero.Invoke();
                playerAnimController.UpdateMeltingAnim(1);
            }
            // eventManager.Raise("玩家血改變了", oldHp, newHp);
            EventAggregator.Instance.ManualTrigger(("Player", EventType.Player, EventBehaviorType.HpChanged));
        }
    }

    private void OnPlayerHpAdded(float addValue)
    {
        float oldHp = hp;
        hp += addValue;
        if (hp >= 100)
            hp = 100;
        if (hp != oldHp)
        {
            EventAggregator.Instance.ManualTrigger(("Player", EventType.Player, EventBehaviorType.HpChanged));
        }
    }


    private void TestAttack1() => OnPlayerAttacked(1);
    private void TestAttack10() => OnPlayerAttacked(10);
    private void TestAttack50() => OnPlayerAttacked(50);
    private void TestAttack200() => OnPlayerAttacked(200);

    public void AddHp(float value)
    {
        OnPlayerHpAdded(value);
    }
}
