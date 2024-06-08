using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        EventAggregator.Instance.ManualTrigger((nameof(GameManager), EventType.System, EventBehaviorType.GameOver));
        
    }
}
