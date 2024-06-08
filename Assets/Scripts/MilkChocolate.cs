﻿using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MilkChocolate : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(MilkChocolate);
    public string ID => nameof(MilkChocolate);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eat = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(MilkChocolate));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.AddHp(15);
            SetEffect(true);
            DestroyAsync(300).Forget();
        }
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(MilkChocolate)}");
    }

}
