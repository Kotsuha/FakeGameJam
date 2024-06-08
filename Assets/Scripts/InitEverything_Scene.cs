using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class InitEverything_Scene : MonoBehaviour
{
    EnemyControl enemy;

    [SerializeField] Enemy timeLineEnemy;

    private void Start()
    {
        InitAsync().Forget();
    }

    int randomAnimalIndex;
    private async UniTask InitAsync()
    {
        await UniTask.DelayFrame(1);

        randomAnimalIndex = Random.Range(0, 8);

        for (int i = 0; i < timeLineEnemy.transform.childCount; i++)
        {
            timeLineEnemy.transform.GetChild(i).gameObject.SetActive(false);
        }
        timeLineEnemy.transform.GetChild(randomAnimalIndex).gameObject.SetActive(true);

        InitEverything();
    }

    private void InitEverything()
    {
        var allRootGos = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var go in allRootGos)
        {
            enemy = go.GetComponentInChildren<EnemyControl>();
            if (enemy)
            {
                enemy.Init(randomAnimalIndex);
                break;
            }
        }
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
        Debug.Log("關掉", enemy.enemy);
        enemy.enemy.gameObject.SetActive(false);
    }
}
