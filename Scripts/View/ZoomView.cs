using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class ZoomView : View
{
    [SerializeField] private Transform _objectToZoom;
    [SerializeField] private float _showZoom;
    [SerializeField] private float _hideZoom;
    [SerializeField] private float _duration;
    [SerializeField] private bool _hideOnStart;
    [SerializeField] private bool _changeActive;

    [SerializeField] private bool _autoHide;
    [SerializeField, ShowIf(nameof(_autoHide))] private float _autoHideDelay;

    [Inject] private Timer _timer;
    
    private bool _isHidden;
    private Tweener _currentTweener;
    private Timer.TimerDelay _currentHidedDelay = null;

    public override bool IsHidden => _isHidden;

    private void OnEnable()
    {
        if (_hideOnStart)
            Hide();
    }

    public override void Show()
    {
        if(_isHidden == false)
            return;
        if(_changeActive)
            _objectToZoom.gameObject.SetActive(true);
        if (_currentHidedDelay != null)
        {
            _currentHidedDelay.Kill();
            _currentHidedDelay = null;
        }

        _isHidden = false;
        _currentTweener.Kill();
        _currentTweener = _objectToZoom.DOScale(Vector3.one*_showZoom, _duration);
        if (_autoHide)
        {
            _currentHidedDelay = _timer.ExecuteWithDelay(Hide, _autoHideDelay);
        }
    }

    public override void Hide()
    {
        _isHidden = true;
        _currentTweener.Kill();
        if(_objectToZoom != null && _objectToZoom.gameObject.activeSelf)
            _currentTweener = _objectToZoom.DOScale(Vector3.one*_hideZoom, _duration).OnComplete(Disable);
    }

    private void Disable()
    {
        if(_changeActive)
            _objectToZoom.gameObject.SetActive(false);
    }

}
