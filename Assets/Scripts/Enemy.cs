using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public event Action OnToughEnemy;

    public Transform target;

    public bool IsTracing => IsTracing;

    float speed = 5;
    Collider collider;

    public bool isTracingTarget;

    public void Init()
    {
        collider = GetComponent<Collider>();
        isTracingTarget = true;

    }

    private void Update()
    {
        if (isTracingTarget)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

    }


    public void SetStartTarcing(bool isTracing)
    {
        isTracingTarget = isTracing;
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        OnToughEnemy?.Invoke();
    }
}
