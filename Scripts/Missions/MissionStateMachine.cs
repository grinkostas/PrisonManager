using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class MissionStateMachine : StateMachineBase<MissionState>
{
    [SerializeField] private MissionView _missionView;
    [SerializeField] private ZoomView _missionZoomView;
    [SerializeField] private float _showDelay;
    [SerializeField] private float _startDelay = 0.5f;
    [Inject] private Timer _timer;
    
    private List<MissionState> _availableStates;

    private void OnEnable()
    {
        
        _availableStates = AllStates.FindAll(x => x.Completed == false);
        foreach (var stateToExit in AllStates.FindAll(x=> x.Completed))
        {
            stateToExit.Exit();
        }
        
        if (_availableStates.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        _timer.ExecuteWithDelay(() =>
        {
            
            _missionZoomView.Show();
            if (_availableStates.Count > 0)
            {
                SwitchState(_availableStates[0]);
            }
        }, _startDelay);
    }

    private void OnDisable()
    {
        _missionZoomView.Hide();
        if(_missionView != null)
            _missionView.gameObject.SetActive(false);
    }

    protected override void OnStateEnter(MissionState state)
    {
        if(state.Completed)
            return;
        foreach (var stateToReceive in AllStates)
        {
            if(stateToReceive != state)
                stateToReceive.ReceiveDestination();
        }
        _missionZoomView.Hide();   
        _missionView.Init(state);
        _timer.ExecuteWithDelay(_missionZoomView.Show, _showDelay);
    }
}
