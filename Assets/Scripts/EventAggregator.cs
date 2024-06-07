using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EventType
{
    Item,
    Enemy,
    System,
}

public enum EventBehaviorType
{
    EnemyAttack,
    ItemBuff,
    ItemDeBuff,
    GameOver,
}

public interface IEventAggregator
{
    string ID { get; }
    EventType Type { get; }
}



public class EventAggregator
{
    Dictionary<(string blockId, EventType addOnType, EventBehaviorType behavior), Delegate> allRegisterEvents;

    public event Action<(string blockId, EventType addonEventType, EventBehaviorType behaviorType)> OnTrigger;

    public static EventAggregator Instance => instance;

    static EventAggregator instance;

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }

        allRegisterEvents = new Dictionary<(string blockId, EventType addOnType, EventBehaviorType behavior), Delegate>();
    }

    public void RegisterAddonEvent(IEventAggregator register, EventBehaviorType behaviorType, Delegate func)
    {
        var key = (register.ID, register.Type, behaviorType);
        if (allRegisterEvents.TryGetValue(key, out var @event))
        {
            allRegisterEvents[(register.ID, register.Type, behaviorType)] = func;
        }
        else
        {
            allRegisterEvents.Add((register.ID, register.Type, behaviorType), func);
        }
    }

    public T InvokeRegisterEvent<T>(string blockId, EventType addonEventType, EventBehaviorType behaviorType)
    {
        if (allRegisterEvents.TryGetValue((blockId, addonEventType, behaviorType), out var action))
        {
            if (action is Func<T> typedAction)
            {
                return typedAction();
            }
            else
            {
                return default;
            }
        }
        else
        {
            return default;
        }
    }

    public void InvokeRegisterEvent(string blockId, EventType addonEventType, EventBehaviorType behaviorType)
    {
        if (allRegisterEvents.TryGetValue((blockId, addonEventType, behaviorType), out var action))
        {
            action?.DynamicInvoke();
        }
    }

    public void UnregisterEvent(IEventAggregator register, EventBehaviorType behaviorType)
    {
        if (allRegisterEvents.ContainsKey((register.ID, register.Type, behaviorType)))
        {
            allRegisterEvents.Remove((register.ID, register.Type, behaviorType));
        }
    }

    public void ManualTrigger((string blockId, EventType addonEventType, EventBehaviorType behaviorType) tuple)
    {
        OnTrigger?.Invoke(tuple);
    }
}
