using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour , IItems  
{
    public static event Action<int> OnFragmentCollected;
    public int fragmentValue = 1;
    public void Collect()
    {
        Destroy(gameObject);
        OnFragmentCollected.Invoke(fragmentValue);
        
    }

}
    
