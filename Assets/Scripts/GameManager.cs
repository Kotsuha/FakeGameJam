using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour, IEventAggregator
{
    private static GameManager instance;

    public string ID => nameof(GameManager);
    public EventType Type => EventType.System;

    Env env;
    EnemyControl enemy;

    [SerializeField] Enemy timeLineEnemy;

    public static GameManager GetInstance()
    {
        if (!instance)
        {
            instance = new GameObject(nameof(GameManager)).AddComponent<GameManager>();
        }
        return instance;
    }

    private void InitEverything()
    {
        var allRootGos = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var go in allRootGos)
        {
            enemy = go.GetComponentInChildren<EnemyControl>();
            if (enemy)
            {
                enemy.Init();
                break;
            }
        }

        env = new Env();
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

        InitEverything();
    }

    private void OnGameOver()
    {
    }


    public void ChangeAnimatorWBeforePlayTimeLine()
    {
        if (timeLineEnemy.PlayableDirector != null)
        {
            timeLineEnemy.SetIsGameOver(true);
            timeLineEnemy.PlayableDirector.Stop();
            TimelineAsset timeline = (TimelineAsset)timeLineEnemy.PlayableDirector.playableAsset;

            var allTrack = timeline.GetOutputTracks().ToList();
            var currentAnimator = new AnimatorControl(timeLineEnemy.GetComponentsInChildren<Animator>().LastOrDefault(animator => animator.enabled == true));

            timeLineEnemy.PlayableDirector.SetGenericBinding(allTrack[0], currentAnimator.GetAnimator());
            var target = EventAggregator.Instance.InvokeRegisterEvent<ITarget>(nameof(Player), EventType.Player, EventBehaviorType.GetSeekTarget);
            timeLineEnemy.PlayableDirector.SetGenericBinding(allTrack[1], target.Trans.gameObject);
            timeLineEnemy.PlayableDirector.SetGenericBinding(allTrack[2], target.Animator);
            timeLineEnemy.PlayableDirector.SetGenericBinding(allTrack[3], target.Animator);

            timeLineEnemy.PlayableDirector.Play();
        }
        enemy.enemy.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventAggregator.Instance.OnTrigger -= OnEventTriggered;
    }

    public Env GetEnv()
    {
        return env;
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


public class Env
{
    public readonly string GroundLayer = "Ground";
}