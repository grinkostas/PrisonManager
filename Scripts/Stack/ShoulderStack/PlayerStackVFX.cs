using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Scripting;

public class PlayerStackVFX : StackCountChangeListner
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private bool _updateCheck;
    
    [SerializeField, AnimatorParam(nameof(_playerAnimator))]
    private string _startStackTrigger;
    
    [SerializeField, AnimatorParam(nameof(_playerAnimator))]
    private string _stopStackTrigger;

    private void Update()
    {
        if(_updateCheck == false)
            return;
        
        if (Stack.ItemsCount > 0)
            _playerAnimator.SetTrigger(_startStackTrigger);
        else
            _playerAnimator.SetTrigger(_stopStackTrigger);
        
    }

    protected override void OnStackCountChanged(int count)
    {
        if (count > 0)
        {
            _playerAnimator.SetTrigger(_startStackTrigger);
        }
        else
        {
            _playerAnimator.SetTrigger(_stopStackTrigger);
        }
    }
}
