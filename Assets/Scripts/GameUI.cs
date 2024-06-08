using DG.Tweening;
using SaintsField;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [BelowButton(nameof(Test_ShowGameTitle))]
    [BelowButton(nameof(Test_HideGameTitle))]
    [SerializeField] private RectTransform titleGroupRectTransform;
    [SerializeField] private CanvasGroup titleGroupCanvasGroup;
    [SerializeField] private RectTransform titleTextGroup;
    [SerializeField] private Button titleButtonGameStart;
    [SerializeField] private EventTrigger titleButtonStartEventTrigger;
    [SerializeField] private Button titleButtonGameExit;
    [SerializeField] private EventTrigger titleButtonExitEventTrigger;

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
        EventAggregator.Instance.OnTrigger += OnEventTriggered;

        Vector3 titleTextGroupInitialScale = titleTextGroup.transform.localScale;
        Vector3 titleTextGroupTweenScale = titleTextGroupInitialScale * 1.5f;
        titleTextGroup.transform.DOScale(titleTextGroupTweenScale, 1).SetLoops(-1, LoopType.Yoyo);

        titleButtonGameStart.onClick.AddListener(() =>
        {
            // 待補
        });
        titleButtonGameExit.onClick.AddListener(() =>
        {
            // 待補
        });
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
    }

    private void OnPlayerHpChanged((string id, EventType eventType, EventBehaviorType behaviorType) tuple)
    {
        var (hp, hpMax) = EventAggregator.Instance.InvokeRegisterEvent<(float hp, float hpMax)>(tuple.id, tuple.eventType, tuple.behaviorType);
        var ratio = hp / hpMax;
        SetHpBarRatio(ratio);
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

    public void ShowGameTitle()
    {
        titleGroupRectTransform.gameObject.SetActive(true);
        titleGroupCanvasGroup.alpha = 0;
        titleGroupCanvasGroup.DOFade(1, 0.3f);
    }

    public void HideGameTitle()
    {
        titleGroupCanvasGroup.DOKill();
        titleGroupCanvasGroup.alpha = 0;
        titleGroupRectTransform.gameObject.SetActive(false);
    }

    private void Test_SetHpBarRatio_0() => SetHpBarRatio(0);
    private void Test_SetHpBarRatio_1() => SetHpBarRatio(1);
    private void Test_SetHpBarRatio_0p5() => SetHpBarRatio(0.5f);
    private void Test_SetScore_0() => SetScore(0);
    private void Test_SetScore_50() => SetScore(50);
    private void Test_SetScore_100() => SetScore(100);
    private void Test_ShowGameOver() => ShowGameOver();
    private void Test_HideGameOver() => HideGameOver();
    private void Test_ShowGameTitle() => ShowGameTitle();
    private void Test_HideGameTitle() => HideGameTitle();
}
