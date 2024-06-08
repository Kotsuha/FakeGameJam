using System;
using UnityEngine;

public class MilkChocolate_Poison : MonoBehaviour, IDebuff, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(MilkChocolate_Poison);
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
        }
    }

    public void DeBuff()
    {

        Debug.Log($"Eat {nameof(MilkChocolate_Poison)}");
    }
}
