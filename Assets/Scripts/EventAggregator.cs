using System;
using System.Collections.Generic;
using UnityEngine;


public class EventAggregator : MonoBehaviour
{
    Dictionary<(string id, EventType type, EventBehaviorType behavior), Delegate> allRegisterEvents;

    public event Action<(string id, EventType eventType, EventBehaviorType behaviorType)> OnTrigger;

    public static EventAggregator Instance => instance;

    static EventAggregator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(this);
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
