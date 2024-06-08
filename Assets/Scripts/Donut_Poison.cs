using System;
using UnityEngine;

public class Donut_Poison : MonoBehaviour, IDebuff, IEventAggregator
{
    public event Action<string> OnGetItem;

    public string Name => nameof(Donut_Poison);
    public string ID => nameof(Donut_Poison);
    public EventType Type => EventType.Item;


    private void Start()
    {
        Action eat = DeBuff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Donut_Poison));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
    }

    public void DeBuff()
    {
    
        Debug.Log($"Eat {nameof(Donut_Poison)}");
    }
}
