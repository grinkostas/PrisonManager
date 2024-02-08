using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using StaserSDK.Utilities;
using Zenject;

public class Presentation : MonoBehaviour
{
    [SerializeField] private GameObject _objectToIntroduce;
    [SerializeField] private float _delay;
    [SerializeField] private float _showTime;
    [SerializeField] private bool _showOnEnable = true;

    [Inject] private InputHandler _inputHandler;
    [Inject] private Timer _timer;

    private Timer.TimerDelay _timerDelay;
    
    private void Awake()
    {
        Hide();
    }

    private void OnEnable()
    {
        if (_showOnEnable)
            Introduce();
    }

    public void Introduce()
    {
        if(Time.timeSinceLevelLoad < 1.0f)
            return;
        _objectToIntroduce.SetActive(true);
        //_inputHandler.DisableHandle(this);
        _timer.ExecuteWithDelay(Hide, _showTime);
    }

    private void Hide()
    {
        if(_timerDelay != null)
            _timerDelay.Kill();
        _objectToIntroduce.SetActive(false);
        _inputHandler.EnableHandle(this);
    }
}
