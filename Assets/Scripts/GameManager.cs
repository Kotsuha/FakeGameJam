using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IEventAggregator
{
    private static GameManager instance;

    public string ID => nameof(GameManager);
    public EventType Type => EventType.System;

    public static GameManager GetInstance()
    {
        if (!instance)
        {
            instance = new GameObject(nameof(GameManager)).AddComponent<GameManager>();
        }
        return instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // 聽「玩家血變了」
        // 入股血量 <= 0 則觸發 Game Over 事件

        Action action = OnGameOver;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.GameOver, action);

        EventAggregator.Instance.OnTrigger += OnEventTriggered;

    }

    private void OnGameOver()
    {
    }

    void OnDestroy()
    {
        EventAggregator.Instance.OnTrigger -= OnEventTriggered;
    }

    private void OnEventTriggered((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        if (tuple.id == nameof(Player) && tuple.eventType == EventType.Player && tuple.behaviorType == EventBehaviorType.HpChanged)
        {
            OnPlayerHpChanged(tuple);
        }
    }

    private void OnPlayerHpChanged((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        var (hp, _) = EventAggregator.Instance.InvokeRegisterEvent<(float hp, float hpMax)>(tuple.id, tuple.eventType, tuple.behaviorType);
        if (hp <= 0)
        {
            EventAggregator.Instance.ManualTrigger(("GameManager", EventType.System, EventBehaviorType.GameOver));
        }
    }
}


public static class Env
{
    public static readonly string GroundLayer = "Ground";
}
