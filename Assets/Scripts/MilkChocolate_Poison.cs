using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MilkChocolate_Poison : ItemBase, IDebuff, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(MilkChocolate_Poison);
    public string ID => nameof(MilkChocolate_Poison);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eat = DeBuff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(MilkChocolate_Poison));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.Attacked(12);
            SetEffect(true);
            DestroyAsync(1000).Forget();
        }
    }

    public void DeBuff()
    {

        Debug.Log($"Eat {nameof(MilkChocolate_Poison)}");
    }
}
