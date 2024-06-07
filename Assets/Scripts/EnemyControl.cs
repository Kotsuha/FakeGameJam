using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Enemy enemy;


    private void Awake()
    {
        enemy = GameObject.Find(nameof(Enemy)).GetComponent<Enemy>();

        enemy.Init();
        enemy.SetStartTarcing(false);


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            enemy.SetStartTarcing(!enemy.IsTracing);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            enemy.SetAnimal(Random.Range(0, 7));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            float isForward = 0;
            float isRight = 90;
            float isBack = 180;

            int randomIndex = Random.Range(0, 3);

            float randomDirection;
            switch (randomIndex)
            {
                case 0:
                    randomDirection = isForward;
                    break;
                case 1:
                    randomDirection = isRight;
                    break;
                case 2:
                    randomDirection = isBack;
                    break;
                default:
                    randomDirection = isForward;
                    break;
            }
            enemy.SetDirection(randomDirection);
        }
    }


}
