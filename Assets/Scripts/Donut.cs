using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Donut : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(Donut);
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
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            SetEffect(true);
            DestroyAsync(300).Forget();
            player.AddHp(7);
        }
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(Donut)}");
    }

}
