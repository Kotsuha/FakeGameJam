using System;
using System.Collections;
using System.Collections.Generic;
using SaintsField;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [BelowButton(nameof(Test_SetHpBarRatio_0))]
    [BelowButton(nameof(Test_SetHpBarRatio_0p5))]
    [BelowButton(nameof(Test_SetHpBarRatio_1))]
    [SerializeField] private Image hpBar;

    [BelowButton(nameof(Test_SetScore_0))]
    [BelowButton(nameof(Test_SetScore_50))]
    [BelowButton(nameof(Test_SetScore_100))]
    [SerializeField] private TMP_Text scoreText;

    [BelowButton(nameof(Test_ShowGameOver))]
    [BelowButton(nameof(Test_HideGameOver))]
    [SerializeField] private RectTransform gameOver;

    // Start is called before the first frame update
    void Start()
    {
        // 聽「玩家血量變了」事件
        // 更新血條
        EventAggregator.Instance.OnTrigger += OnEventTriggered;
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
        var (hp, hpMax) = EventAggregator.Instance.InvokeRegisterEvent<(float hp, float hpMax)>(tuple.id, tuple.eventType, tuple.behaviorType);
        var ratio = hp / hpMax;
        SetHpBarRatio(ratio);
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
        gameOver.gameObject.SetActive(true);
    }

    private void HideGameOver()
    {
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
