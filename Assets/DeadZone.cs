using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("你碰到 Dead Zone 了");
            EventAggregator.Instance.ManualTrigger((nameof(GameManager), EventType.System, EventBehaviorType.GameOver));
        }
    }
}
