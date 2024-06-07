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
    }


}
