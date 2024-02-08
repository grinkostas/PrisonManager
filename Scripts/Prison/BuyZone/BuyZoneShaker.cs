using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Utilities;
using Zenject;

public class BuyZoneShaker : MonoBehaviour
{
    [SerializeField] private Shaker _shaker;
    [SerializeField] private float _shakeDelay;
    [SerializeField] private PlayerZone _playerZone;
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private BuyFx _buyFx;

    [Inject] private Timer _timer;
    
    private List<Timer.TimerDelay> _timers = new List<Timer.TimerDelay>();

    private void OnEnable()
    {
        _buyFx.SpawnedNode += OnSpawnedNode;
        _playerZone.OnExit += OnExit;
        _buyZone.Bought += KillDelays;
    }

    private void OnDisable()
    {
        _buyFx.SpawnedNode -= OnSpawnedNode;
        _playerZone.OnExit -= OnExit;
        _buyZone.Bought -= KillDelays;
    }
    
    private void OnSpawnedNode(GameObject node)
    {
        var delay = _timer.ExecuteWithDelay(_shaker.Shake, _shakeDelay);
        _timers.Add(delay);
    }

    private void OnExit(InteractableCharacter character)
    {
        KillDelays();
    }

    private void KillDelays()
    {
        foreach (var timerDelay in _timers)
        {
            timerDelay.Kill();
        }
    }
}
