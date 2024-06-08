using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MilkChocolate : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(MilkChocolate);
    public string ID => nameof(MilkChocolate);
    public EventType Type => EventType.Item;

    float score;
    private void Start()
    {
        Action eat = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);
        score = 15;

        Func<float> sendScore = GetScore;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.Score, sendScore);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(MilkChocolate));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.AddHp(score);
            SetEffect(true);
            DestroyAsync(300).Forget();
        }

        EventAggregator.Instance.ManualTrigger((nameof(MilkChocolate), EventType.Item, EventBehaviorType.Score));
    }

    public float GetScore()
    {
        return score;
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(MilkChocolate)}");
    }

}
