using System;
using UnityEngine;

public interface IEnemy
{
    event Action OnTouchEnemy;
    void SetStartTarcing(bool isTracing);
    void SetTarget(ITarget target);
    void SetSpeed(float value);
}

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour, IEnemy, IEventAggregator
{
    public event Action OnTouchEnemy;

    public bool IsTracing => isTracingTarget;
    public float Speed => speed;

    public string ID => nameof(Enemy);

    public EventType Type => EventType.Enemy;

    ITarget target;
    float speed = 5;
    Collider collider;

    public bool isTracingTarget;

    public void Init()
    {
        collider = GetComponent<Collider>();
        isTracingTarget = true;
        Action eatPlayerAction = EatPlayer;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.EnemyAttack, eatPlayerAction);
    }

    private void Update()
    {
        if (isTracingTarget)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

    }


    public void SetStartTarcing(bool isTracing)
    {
        isTracingTarget = isTracing;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void SetTarget(ITarget target)
    {
        this.target = target;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        OnTouchEnemy?.Invoke();

        if (other.gameObject.name == nameof(Chocolate))
        {
            EventAggregator.Instance.InvokeRegisterEvent(nameof(Chocolate), EventType.Item, EventBehaviorType.ItemBuff);
        }

    }

    private void EatPlayer()
    {
        Debug.Log("Eat player");
    }
}
