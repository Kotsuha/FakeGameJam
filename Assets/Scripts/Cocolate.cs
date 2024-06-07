using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chocolate : MonoBehaviour, IItem
{
    public string Name => nameof(Chocolate);

    public event Action<string> OnGetItem;



    private void OnTriggerEnter(Collider other)
    {
        OnGetItem?.Invoke(nameof(Chocolate));
    }

}
