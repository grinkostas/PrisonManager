using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using Zenject;

public class MaximizeInputEnabler : MonoBehaviour
{
    [Inject] private InputHandler _inputHandler;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _inputHandler.EnableMaximization();
        }
    }
}
