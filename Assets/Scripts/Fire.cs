using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IDebuffItem, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Fire);
    public EventType Type => EventType.Item;
    public string ID => Name;

    private void Awake()
    {
        Action<IPlayer> attackAction = DeBuff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemDeBuff, attackAction);
    }

    public void DeBuff(IPlayer player)
    {
        Debug.Log("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Fire));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemDeBuff));
    }
}
