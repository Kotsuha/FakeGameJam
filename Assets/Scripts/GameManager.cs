using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (!instance)
        {
            instance = new GameObject(nameof(GameManager)).AddComponent<GameManager>();
        }
        return instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
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
    }
}
