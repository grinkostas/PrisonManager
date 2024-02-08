using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public abstract class PointerState : State
{
    [SerializeField, ShowIf(nameof(_spawnVerticalPointer))] 
    private float _yOffset = 0.0f;
    [SerializeField] private bool _delayed;
    [SerializeField] private int _pointerPriority = 2;

    [SerializeField, ShowIf(nameof(_delayed))]
    private float _delay = 0.0f;

    [SerializeField] private bool _spawnVerticalPointer = true;
    
    [Inject] private AboveArrow _verticalPointerPrefab;
    [Inject] private Timer _timer;
    [Inject] protected Player Player;
    
    private AboveArrow _verticalPointer;
    
    protected abstract Transform Target { get; }
    private bool _entered = false;

    private Transform _currentTarget;

    private Timer.TimerDelay _actualizeDelay;


    private void OnDisable()
    {
        ReceiveDestination();
    }
    
    public override void OnEnter()
    {
        if(DebugMode)
            Debug.Log($"{gameObject.name} On Enter");
        if(enabled == false)
            return;
        if(DebugMode)
            Debug.Log($"{gameObject.name} enabled");
        ReceiveDestination();
        if(_delayed == false)
            ActualizePointer();
        else
            _actualizeDelay = _timer.ExecuteWithDelay(ActualizePointer, _delay, TimeScale.Scaled);
    }

    protected void ActualizePointer()
    {
        if(enabled == false)
            return;
        if(_currentTarget != null)
            ReceiveDestination();
        _currentTarget = Target;
        Player.Pointer.SetDestination(_currentTarget, _pointerPriority);
        if (_spawnVerticalPointer)
        {
            if (_verticalPointer != null)
                _verticalPointer.transform.SetParent(_currentTarget);
            else
                _verticalPointer = Instantiate(_verticalPointerPrefab, _currentTarget);
            
            _verticalPointer.transform.localPosition = Vector3.up * _yOffset;
        }
    }

    public override void OnExit()
    {
        if(_actualizeDelay != null)
            _actualizeDelay.Kill();
        ReceiveDestination();
    }

    public void ReceiveDestination()
    {
        Player.Pointer.ReceiveDestination(_currentTarget);

        if (_verticalPointer != null)
        {
            Destroy(_verticalPointer.gameObject);
            _verticalPointer = null;
        }
    }
}
