using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;

public class IceRink : MonoBehaviour
{
    [SerializeField] private CharacterControllerMovementAdvanced _characterController;
    [SerializeField] private float _angularSpeedChange;
    [SerializeField] private NumericAction _changeAction;

    private FloatModifier _angularSpeedModifer;
    private bool _addedModifier = false;
    
    
    private void Awake()
    {
        _angularSpeedModifer = new FloatModifier(_angularSpeedChange, _changeAction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_addedModifier)
            return;
        if (other.TryGetComponent(out Player player))
        {
            _addedModifier = true;
            _characterController.AngularSpeed.AddModifier(_angularSpeedModifer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _addedModifier = false;
            _characterController.AngularSpeed.RemoveModifier(_angularSpeedModifer);
        }
    }
}
