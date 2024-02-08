using System;
using UnityEngine;
using System.Collections;
using StaserSDK.Views;
using StaserSDK.Utilities;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using Zenject;

public class LevelUpPopup : Popup
{
    [SerializeField] private float _subscribeDelay = 0.25f;
    [SerializeField] private float _showDelay = 0.5f;
    
    [SerializeField] private Transform _startLevelViewPosition;
    [SerializeField] private Vector3 _startScale;
    
    [SerializeField] private LevelView _levelView;
    [SerializeField] private LevelView _popupLevelView;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _slideDuration;
    
    
    [Inject] private LevelSystem _levelSystem;
    [Inject] private Timer _timer;
    private void OnEnable()
    {
        _timer.ExecuteWithDelay(() => _levelSystem.OnLevelUp += ShowPopup, _subscribeDelay);
        _popupLevelView.CanvasGroup.alpha = 0.0f;
    }

    private void OnDisable()
    {
        _levelSystem.OnLevelUp -= ShowPopup;
    }

    public void ShowPopup()
    {
        _timer.ExecuteWithDelay(Show, _showDelay);
    }

    protected override void OnShow()
    {
        _popupLevelView.transform.position = _startLevelViewPosition.position;
        _popupLevelView.transform.localScale = _startScale;
        _popupLevelView.Actualize();
        this.FadeValue(0, 1, opacity => _popupLevelView.CanvasGroup.alpha = opacity, _fadeDuration );
    }

    protected override void OnHide()
    {
        _popupLevelView.transform.DOLocalMove(_levelView.transform.localPosition, _slideDuration).OnComplete(() =>
        {
            _levelView.Actualize();
            _popupLevelView.CanvasGroup.alpha = 0.0f;
        });
        _popupLevelView.transform.DOScale(_levelView.transform.localScale, _slideDuration);
    }
    
    
}
