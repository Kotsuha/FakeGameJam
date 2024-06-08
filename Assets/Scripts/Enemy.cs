using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public interface IEnemy
{
    event Action OnTouchEnemy;
    void SetStartTarcing(bool isTracing);
    void SetTarget(ITarget target);
    void SetSpeed(float value);
    float AttackPlayer();
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
        target = EventAggregator.Instance.InvokeRegisterEvent<ITarget>(nameof(Player), EventType.Player, EventBehaviorType.GetSeekTarget);
        attackValue = 30;

        EventAggregator.Instance.OnTrigger += DoGameOver;
    }

    private void DoGameOver((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        if (tuple.id == nameof(GameManager) && tuple.eventType == EventType.System && tuple.behaviorType == EventBehaviorType.GameOver)
        {
            OnGameOver(tuple);
        }
    }

    private void OnGameOver((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        isTracingTarget = false;
        currentAnimator.Play(AnimatorControl.Spin);
        isGameOver = true;
    }

    bool isGameOver = false;

    private void Update()
    {
        if(isGameOver)
        {
            return;
        }
        if (isTracingTarget)
        {
            currentAnimator.Play(AnimatorControl.Walk);
            Seek();
            //SeekAsync().Forget();
        }
    }

    private void Seek()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.Trans.position, speed * Time.deltaTime);

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 10;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 300, layerMask: LayerMask.GetMask(GameManager.GetInstance().GetEnv().GroundLayer)))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        Vector3 targetPosition = new Vector3(target.Trans.position.x, transform.position.y, target.Trans.position.z);
        transform.DOLookAt(targetPosition, speed * Time.deltaTime).SetEase(Ease.InExpo);
    }

    private async UniTask SeekAsync()
    {
        await UniTask.Delay(300);
        Seek();
    }

    private float CheckDistance()
    {
        return (target.Trans.position - transform.position).magnitude;
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
        if (isGameOver)
        {
            return;
        }

        Debug.Log("Enter");
        OnTouchEnemy?.Invoke();

        if (other.gameObject.name == nameof(Chocolate))
        {
            EventAggregator.Instance.InvokeRegisterEvent(nameof(Chocolate), EventType.Item, EventBehaviorType.ItemBuff);
            EatPlayer();
        }

        if (other.gameObject.name == nameof(Player))
        {
            var player = other.gameObject.GetComponent<IPlayer>();
            player.Attack(AttackPlayer());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isGameOver)
        {
            return;
        }

        if (other.gameObject.name == nameof(Player))
        {
            EatPlayer();
        }
    }

    public void SetAttackValue(float value)
    {
        attackValue = value;
    }

    float attackValue = 30;
    public float AttackPlayer()
    {
        return attackValue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isGameOver)
        {
            return;
        }

        currentAnimator.Play(currentAnimator.GetRandomIdle());
        ReSeekTrget().Forget();
    }

    private async UniTask ReSeekTrget()
    {
        await UniTask.Delay(1000);
        currentAnimator.Play(UnityEngine.Random.Range(0, 2) == 1 ? AnimatorControl.Fear : AnimatorControl.Jump);
        await UniTask.Delay(1000);
        isTracingTarget = true;
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
    public static readonly string Jump = "Jump";
    public static readonly string Fear = "Fear";

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