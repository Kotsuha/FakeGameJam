using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Enemy enemy;

    public void Init()
    {
        enemy = Instantiate(enemy);
        enemy.transform.position = transform.position;
        enemy.Init();
        SetSeekAsync(enemy, true).Forget();
    }

    private async UniTask SetSeekAsync(Enemy enemy,bool isSeeking)
    {
        await UniTask.DelayFrame(3);
        enemy.SetStartTarcing(isSeeking);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        enemy.SetStartTarcing(!enemy.IsTracing);
    //    }

    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        enemy.SetAnimal(Random.Range(0, 7));
    //    }

    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        float isForward = 0;
    //        float isRight = 90;
    //        float isBack = 180;

    //        int randomIndex = Random.Range(0, 3);

    //        float randomDirection;
    //        switch (randomIndex)
    //        {
    //            case 0:
    //                randomDirection = isForward;
    //                break;
    //            case 1:
    //                randomDirection = isRight;
    //                break;
    //            case 2:
    //                randomDirection = isBack;
    //                break;
    //            default:
    //                randomDirection = isForward;
    //                break;
    //        }
    //        enemy.SetDirection(randomDirection);
    //    }
    //}


}
