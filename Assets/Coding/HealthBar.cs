using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject MaskPrefab;
    public PlayerHealth playerHealth;
    List<HealthMask> Masks = new List<HealthMask>();

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += DrawMasks;
        PlayerHealth.OnPlayerHeal += DrawMasks;
        
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= DrawMasks;
        PlayerHealth.OnPlayerHeal -= DrawMasks;
    }
    private void Start()
    {
        DrawMasks();
    }



    public void DrawMasks()
    {
        ClearMasks();

        float maxHealthRemainder = playerHealth.maxHealth % 3;
        int masksToMake = (int)((playerHealth.maxHealth/3) + maxHealthRemainder);
        for (int i = 0; i < masksToMake; i++)
        {
            CreateEmptyMasks();

        }

        for(int i = 0;i < Masks.Count; i++)
        {
            int maskStatusRemainder = (int)Mathf.Clamp(playerHealth.health - (i * 3), 0, 3);
            Masks[i].UpdateMaskState((MaskState)maskStatusRemainder);
        }
    }

    public void CreateEmptyMasks()
    {
        GameObject newMask = Instantiate(MaskPrefab);
        newMask.transform.SetParent(transform);

        HealthMask maskComponent = newMask.GetComponent<HealthMask>();
        maskComponent.UpdateMaskState(MaskState.None);
        Masks.Add(maskComponent);
    }
    public void ClearMasks()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        Masks = new List<HealthMask>();
    }


}
