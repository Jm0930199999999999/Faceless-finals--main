using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItems
{
    public int healAmount = 3;
    public static event Action<int> OnHealthCollect;
    public void Collect()
    {
        OnHealthCollect.Invoke(healAmount);
        Destroy(gameObject);
    }
}
