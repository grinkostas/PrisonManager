using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using StaserSDK.Views;
using Zenject;

public class MovePresenter : ViewPresenter
{
    [SerializeField] private PresenterAction _onStartMove;
    [SerializeField] private PresenterAction _onMove;
    [SerializeField] private PresenterAction _onStopMove;

    [Inject] private InputHandler _inputHandler;

    private void OnEnable()
    {
        _inputHandler.OnStartMove.AddListener(OnStartMove);
        _inputHandler.OnMove.AddListener(OnMove);
        _inputHandler.OnStopMove.AddListener(OnStopMove);
        
    }

    private void OnDisable()
    {
        _inputHandler.OnStartMove.RemoveListener(OnStartMove);
        _inputHandler.OnMove.RemoveListener(OnMove);
        _inputHandler.OnStopMove.RemoveListener(OnStopMove);
    }

    private void OnStartMove() => HandlePresenterAction(_onStartMove);
    private void OnStopMove()=> HandlePresenterAction(_onStopMove);
    private void OnMove(Vector3 input) => HandlePresenterAction(_onMove);
    
}
