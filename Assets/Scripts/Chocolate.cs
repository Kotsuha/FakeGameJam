using System;
using UnityEngine;

public class Chocolate : MonoBehaviour, IBuffItem, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Chocolate);
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
    }

    public void Buff()
    {
        Debug.Log("Eat chocholate");
    }

}
