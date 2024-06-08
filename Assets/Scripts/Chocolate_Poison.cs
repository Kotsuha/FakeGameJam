using System;
using UnityEngine;

public class Chocolate_Poison : MonoBehaviour, IDebuff, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Chocolate_Poison);
    public string ID => nameof(Chocolate_Poison);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eatChocolate = DeBuff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eatChocolate);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Chocolate));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.Attacked(30);
        }
    }

    public void DeBuff()
    {
        Debug.Log("Poison");
    }
}