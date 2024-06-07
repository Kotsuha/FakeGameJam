using System;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] List<Animator> allAnimal;

    public bool IsTracing => isTracingTarget;
    public float Speed => speed;
    public string ID => nameof(Enemy);

    public EventType Type => EventType.Enemy;

    ITarget target;
    float speed = 5;
    AnimatorControl currentAnimator;
    public bool isTracingTarget;

    public void Init()
    {
        isTracingTarget = true;
        Action eatPlayerAction = EatPlayer;
        EventAggregator.Instance.RegisterAddonEvent(this, EventBehaviorType.EnemyAttack, eatPlayerAction);

        currentAnimator = new AnimatorControl(GetComponentInChildren<Animator>());
        currentAnimator.Play(AnimatorControl.Walk);
    }

    private void Update()
    {
        if (isTracingTarget)
        {
            currentAnimator.Play(AnimatorControl.Walk);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

    }

    public void SetAnimal(int index)
    {
        allAnimal.ForEach(go => go.gameObject.SetActive(false));
        allAnimal[index].gameObject.SetActive(true);
    }


    public void SetStartTarcing(bool isTracing)
    {
        isTracingTarget = isTracing;
        currentAnimator.Play(isTracingTarget == true ? AnimatorControl.Walk : currentAnimator.GetRandomIdle());
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void SetTarget(ITarget target)
    {
        this.target = target;
    }

    public void SetDirection(float angle)
    {
        transform.DORotate(new Vector3(0f, angle), 0.6f).SetEase(Ease.InElastic);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        OnTouchEnemy?.Invoke();

        if (other.gameObject.name == nameof(Chocolate))
        {
            EventAggregator.Instance.InvokeRegisterEvent(nameof(Chocolate), EventType.Item, EventBehaviorType.ItemBuff);
            EatPlayer();
        }

        if (other.gameObject.name == nameof(Chocolate))
        {
            EatPlayer();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        currentAnimator.Play(currentAnimator.GetRandomIdle());
    }

    private void EatPlayer()
    {
        Debug.Log("Eat player");
        currentAnimator.Play(AnimatorControl.Eat);
        isTracingTarget = false;
    }

    private void EatFood()
    {
        currentAnimator.Play(AnimatorControl.Eat);
    }
}


public class AnimatorControl
{
    public static readonly string Eat = "Eat";
    public static readonly string Walk = "Walk";
    public static readonly string Idle_A = "Idle_A";
    public static readonly string Idle_B = "Idle_B";
    public static readonly string Idle_C = "Idle_C";

    public static readonly string Spin = "Spin";
    public static readonly string Death = "Death";

    Animator animator;
    public AnimatorControl(Animator animator)
    {
        this.animator = animator;
    }

    public void Play(string key)
    {
        animator.Play(key);
    }

    public string GetRandomIdle()
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);

        string reandmIdle = string.Empty;
        switch (randomIndex)
        {
            case 0:
                reandmIdle = AnimatorControl.Idle_A;
                break;
            case 1:
                reandmIdle = AnimatorControl.Idle_B;
                break;
            case 2:
                reandmIdle = AnimatorControl.Idle_C;
                break;
        }
        return reandmIdle;
    }
}