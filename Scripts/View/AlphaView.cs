using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AlphaView : View
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration;
    [SerializeField] private bool _hideOnStart;

    private bool _isHidden;
    private Tweener _currentTweener;

    public override bool IsHidden => _isHidden;

    private void OnEnable()
    {
        if (_hideOnStart)
            Hide();
    }

    public override void Show()
    {
        _isHidden = false;
        _currentTweener.Kill();
        _currentTweener = _canvasGroup.DOFade(1, _duration);

    }

    public override void Hide()
    {
        _isHidden = true;
        _currentTweener.Kill();
        _currentTweener = _canvasGroup.DOFade(0, _duration);
    }

    public void ForceHide()
    {
        _isHidden = true;
        _currentTweener.Kill();
        _canvasGroup.alpha = 0.0f;
    }
}
