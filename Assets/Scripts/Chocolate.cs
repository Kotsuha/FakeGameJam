using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Chocolate : ItemBase, IBuffItem, IEventAggregator
{
    public override event Action<string> OnGetItem;

    public override string Name => nameof(Chocolate);
    public string ID => nameof(Chocolate);
    public EventType Type => EventType.Item;


    float score;
    private void Start()
    {
        Action eatChocolate = Buff;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.ItemBuff, eatChocolate);

        score = 30;
        Func<float> sendScore = GetScore;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.Score, sendScore);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Chocolate));
        EventAggregator.Instance.ManualTrigger((ID, EventType.Item, EventBehaviorType.ItemBuff));
        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.AddHp(score);
            SetEffect(true);
            DestroyAsync(300).Forget();
        }

        EventAggregator.Instance.ManualTrigger((nameof(Chocolate), EventType.Item, EventBehaviorType.Score));
    }

    public void Buff()
    {
        Debug.Log("Eat chocholate");
    }

    public float GetScore()
    {
        return score;
    }
}
