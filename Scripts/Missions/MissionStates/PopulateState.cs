using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PopulateState : MissionState
{
    [SerializeField] private Transform _roomTransform;

    [Inject] private HumanSpawner _humanSpawner;
    [Inject] private Stack _shoulderStack;
    protected override Transform Target => 
        _shoulderStack.ItemsCount > 0 ? _roomTransform : _humanSpawner.GetNearestTarget();

    public override void OnEnter()
    {
        base.OnEnter();
        _shoulderStack.CountChanged += OnStackCountChanged;
    }

    public override void OnExit()
    {
        base.OnExit();
        _shoulderStack.CountChanged -= OnStackCountChanged;
    }
    
    private void OnStackCountChanged(int count)
    {
        ActualizePointer();
    }
    
}
