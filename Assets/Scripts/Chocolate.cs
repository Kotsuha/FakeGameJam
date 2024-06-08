using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Chocolate : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(Chocolate);
    public string ID => nameof(Chocolate);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eatChocolate = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eatChocolate);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Chocolate));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.AddHp(30);
            SetEffect(true);
            DestroyAsync(300    ).Forget();
        }
    }

    public void Buff()
    {
        Debug.Log("Eat chocholate");
    }
}
