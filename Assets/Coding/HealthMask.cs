using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMask : MonoBehaviour
{
    public Sprite maskIntact, maskChipped, maskBroken, maskNone;
    Image MaskImage;

    private void Awake()
    {
        MaskImage = GetComponent<Image>();

    }
    public void UpdateMaskState(MaskState state)
    {
        switch (state)
        {
            case MaskState.Intact:
                MaskImage.sprite = maskIntact;
                break;
            case MaskState.Chipped:
                MaskImage.sprite = maskChipped;
                break;
            case MaskState.Broken:
                MaskImage.sprite = maskBroken;
                break;
            case MaskState.None:
                MaskImage.sprite = maskNone;
                break;
        }
    } 
}

public enum MaskState
{
    Intact=3,
    Chipped=2,
    Broken=1,
    None=0
}