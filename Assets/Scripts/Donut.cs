using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Donut : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(Donut);
    public string ID => nameof(Donut);
    public EventType Type => EventType.Item;

    float score;
    private void Start()
    {
        Action eat = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eat);

        score = 7;
        Func<float> sendScore = GetScore;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.Score, sendScore);
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
            player.AddHp(score);
        }

        EventAggregator.Instance.ManualTrigger((nameof(Donut), EventType.Item, EventBehaviorType.Score));
    }

    public void Buff()
    {
        Debug.Log($"Eat {nameof(Donut)}");
    }

    public float GetScore()
    {
        return score;
    }
}
