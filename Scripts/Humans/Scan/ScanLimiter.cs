using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Utilities;
using Zenject;

public class ScanLimiter : MonoBehaviour
{
    [SerializeField] private int _maxCriminals;
    [SerializeField] private Scannable _scannable;

    [Inject] private Timer _timer;
    
    public List<RunState> _blockers = new List<RunState>();

    private void OnEnable()
    {
        RunState.StartRun += OnStartRun;
        RunState.EndRun += OnStopRun;
    }
    
    private void OnDisable()
    {
        RunState.StartRun -= OnStartRun;
        RunState.EndRun -= OnStopRun;
    }

    private void FixedUpdate()
    {
        if (_blockers.Count < _maxCriminals)
        {
            _scannable.enabled = true;
            if (_scannable.Collider.enabled == false)
                _scannable.Collider.enabled = true;
        }
        else
            _scannable.enabled = false;
    }

    private void OnStartRun(RunState state)
    {
        if (_blockers.Contains(state) == false)
        {
            _blockers.Add(state);
            _timer.ExecuteWithDelay(() => _blockers.Remove(state), state.RunTime);
        }
    }

    private void OnStopRun(RunState state)
    {
        if(_blockers.Contains(state))
            _blockers.Remove(state);
    }
}
