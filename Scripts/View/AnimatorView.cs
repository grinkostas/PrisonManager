using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class AnimatorView : View
{
    [SerializeField] private Animator _animator;

    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _showTrigger;
    
    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _hideTrigger;

    private bool _isHidden = false;

    public override bool IsHidden => _isHidden;

    public override void Show()
    {
        _isHidden = false;
        _animator.SetTrigger(_showTrigger);
    }

    public override void Hide()
    {
        _isHidden = true;
        _animator.SetTrigger(_hideTrigger);
    }
}
