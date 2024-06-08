using System;
using DG.Tweening;
using SaintsField;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour, IEventAggregator
{
    [BelowButton(nameof(Test_SetHpBarRatio_0))]
    [BelowButton(nameof(Test_SetHpBarRatio_0p5))]
    [BelowButton(nameof(Test_SetHpBarRatio_1))]
    [SerializeField] private Image hpBar;
    [SerializeField] private TMP_Text hpText;

    [BelowButton(nameof(Test_SetScore_0))]
    [BelowButton(nameof(Test_SetScore_50))]
    [BelowButton(nameof(Test_SetScore_100))]
    [SerializeField] private TMP_Text scoreText;

    [BelowButton(nameof(Test_ShowGameOver))]
    [BelowButton(nameof(Test_HideGameOver))]
    [SerializeField] private RectTransform gameOver;

    public string ID => nameof(UI_Game);

    public EventType Type => EventType.System;

    float score;

    // Start is called before the first frame update
    void Start()
    {
        EventAggregator.Instance.OnTrigger += OnEventTriggered;

        score = 0;
    }

    void OnDestroy()
    {
        EventAggregator.Instance.OnTrigger -= OnEventTriggered;
    }

    private void OnEventTriggered((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        // 聽「玩家血量變了」事件，更新血條
        if (tuple.id == nameof(Player) && tuple.eventType == EventType.Player && tuple.behaviorType == EventBehaviorType.HpChanged)
        {
            OnPlayerHpChanged(tuple);
        }
        // 聽「遊戲結束」事件，出現 Game Over 字樣
        if (tuple.id == nameof(GameManager) && tuple.eventType == EventType.System && tuple.behaviorType == EventBehaviorType.GameOver)
        {
            OnGameOver(tuple);
        }
        // 聽「遊戲開始」事件，隱藏 Game Over 字樣
        if (tuple.id == nameof(GameManager) && tuple.eventType == EventType.System && tuple.behaviorType == EventBehaviorType.GameStart)
        {
            OnGameStart(tuple);
        }

        if (tuple.eventType == EventType.Item && tuple.behaviorType == EventBehaviorType.Score)
        {
            var itemScore = EventAggregator.Instance.InvokeRegisterEvent<float>(tuple.id, EventType.Item, EventBehaviorType.Score);
            Debug.Log("Get Score");
            score += itemScore;
            SetScore(score);
        }
    }

    private void OnPlayerHpChanged((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        var (hp, hpMax) = EventAggregator.Instance.InvokeRegisterEvent<(float hp, float hpMax)>(tuple.id, tuple.eventType, tuple.behaviorType);
        var ratio = hp / hpMax;
        SetHpBarRatio(ratio);
        var hpText = ratio * 100;
        SetHpText(hp == 0 ? "0" : hp == 100 ? "100" : Mathf.FloorToInt(hpText).ToString());
    }

    private void SetHpText(string text)
    {
        hpText.text = text;
    }

    private void OnGameOver((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        ShowGameOver();
    }

    private void OnGameStart((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        HideGameOver();
    }

    private void SetHpBarRatio(float value)
    {
        hpBar.fillAmount = value;
    }

    private void SetScore(float value)
    {
        scoreText.text = value.ToString();
    }

    private void ShowGameOver()
    {
        // Debug.Log("顯示 Game Over");
        gameOver.gameObject.SetActive(true);
    }

    private void HideGameOver()
    {
        // Debug.Log("隱藏 Game Over");
        gameOver.gameObject.SetActive(false);
    }

    private void Test_SetHpBarRatio_0() => SetHpBarRatio(0);
    private void Test_SetHpBarRatio_1() => SetHpBarRatio(1);
    private void Test_SetHpBarRatio_0p5() => SetHpBarRatio(0.5f);
    private void Test_SetScore_0() => SetScore(0);
    private void Test_SetScore_50() => SetScore(50);
    private void Test_SetScore_100() => SetScore(100);
    private void Test_ShowGameOver() => ShowGameOver();
    private void Test_HideGameOver() => HideGameOver();
}
