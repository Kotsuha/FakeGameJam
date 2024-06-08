using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RoadSection : MonoBehaviour, IEventAggregator
{
    public RoadSectionTrigger middleRoadTrigger;

    [SerializeField] private Transform nextRoadSectionPos;

    private bool hasReachMiddle;

    public Transform NextRoadSectionPos => nextRoadSectionPos;

    public string ID => nameof(RoadSection);
    public EventType Type => EventType.System;

    public UnityEvent onPlayerReachMid;

    void Awake()
    {
        middleRoadTrigger.onPlayerEnter.AddListener(OnPlayerEnterMiddle);
    }

    private void OnPlayerEnterMiddle()
    {
        if (!hasReachMiddle)
        {
            hasReachMiddle = true;
            middleRoadTrigger.transform.DOLocalJump(middleRoadTrigger.transform.localPosition, 0.6f, 1, 0.5f);
            onPlayerReachMid.Invoke();
        }
    }
}
