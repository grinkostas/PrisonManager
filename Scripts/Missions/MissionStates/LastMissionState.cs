using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LastMissionState : MissionState
{
    [SerializeField] private MissionStateMachine _stateMachine;
    public override void OnEnter()
    {
        _stateMachine.gameObject.SetActive(false);
        Exit();
    }

}
