using System;
using UnityEngine;

public class MilkChocolate : MonoBehaviour, IBuffItem, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(MilkChocolate);
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
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(MilkChocolate)}");
    }

}
