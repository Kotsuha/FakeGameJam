using System;
using UnityEngine;

public class Donut : MonoBehaviour, IBuffItem, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Donut);
    public string ID => nameof(Donut);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eat = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Donut));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(Donut)}");
    }

}
