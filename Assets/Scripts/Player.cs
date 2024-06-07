using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float hp;

    public float Hp => hp;

    void Start()
    {
        // eventManager.聽("攻擊玩家", "多少");
    }

    private void OnPlayerAttacked(float attackPower)
    {
        float oldHp = hp;
        hp -= attackPower;
        if (hp < 0)
            hp = 0;
        if (hp != oldHp)
        {
            // eventManager.Raise("玩家血改變了", oldHp, newHp);
        }
    }
}
