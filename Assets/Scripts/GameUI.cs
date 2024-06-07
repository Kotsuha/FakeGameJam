using System;
using System.Collections;
using System.Collections.Generic;
using SaintsField.Playa;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private TMP_Text scoreText;

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
            var (hp, hpMax) = EventAggregator.Instance.InvokeRegisterEvent<(float hp, float hpMax)>(tuple.id, tuple.eventType, tuple.behaviorType);
            var ratio = hp / hpMax;
            SetHpBarRatio(ratio);
        }
    }

    private void SetHpBarRatio(float value)
    {
        hpBar.fillAmount = value;
    }

    private void SetScore(float value)
    {
        scoreText.text = value.ToString();
    }

    private void Test_SetHpBarRatio()
    {

    }
    private void Test_SetScore()
    {

    }

    [Button]
    private void OnButtonParams(UnityEngine.Object myObj, int myInt, string myStr = "hi")
    {
        Debug.Log($"{myObj}, {myInt}, {myStr}");
    }
}
