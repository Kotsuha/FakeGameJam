using System;
using UnityEngine;

public class Sugar : MonoBehaviour, IBuffItem, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Sugar);
    public string ID => nameof(Sugar);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eat = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Sugar));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));

        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.AddHp(9);
        }
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(Sugar)}");

    }
}
