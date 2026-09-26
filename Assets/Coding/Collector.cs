using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItems item = collision.GetComponent<IItems>();
        if (item != null)
        {
            item.Collect();
            SoundEffectManager.Play("Fragment");
        }
    } 
}
