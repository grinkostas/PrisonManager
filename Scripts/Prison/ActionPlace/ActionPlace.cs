using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.Events;
using Zenject;
using StaserSDK.Utilities;

public class ActionPlace : ActionPlaceBase
{
    [SerializeField] private ActionPlaceType _type;
    
    [Header("Interact Time")]
    [SerializeField] private bool _random;

    [SerializeField] private bool _limitedTime = true;
    [SerializeField, HideIf(nameof(_random))] 
    private float _interactTime;
    [SerializeField, ShowIf(nameof(_random))]
    private Vector2 _interactTimeRange;
    
    [Header("Move Settings")]
    [SerializeField] private float _moveTime = 0.0f;
    [SerializeField] private bool _move = true;
    [SerializeField] private bool _rotate = true;
    [Header("Points")]
    [SerializeField] private Transform _interactPoint;
    [SerializeField, ShowIf(nameof(ActionPointEditorShowCondition))] 
    private Transform _actionPoint;
    [SerializeField, ShowIf(nameof(ActionPointEditorShowCondition))] 
    private Transform _exitPoint;
    
    [Header("Animations")]
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _disableAgent = true;
    
    [SerializeField, AnimatorParam(nameof(_animator))] 
    private string _startAnimationTrigger;

    [SerializeField, AnimatorParam(nameof(_animator))]
    private string _stopAnimationTrigger;
    
    
    [Inject] private Timer _timer;
    
    public Prisoner _upcomingPrisoner = null;
    public Prisoner _previousPrisoner = null;

    private Timer.TimerDelay _kickDelay = null;

    private bool ActionPointEditorShowCondition => _move || _rotate;
    public Prisoner CurrentPrisoner = null;
    

    public override float InteractTime => _interactTime;
    public override ActionPlaceType Type => _type;
    public override Transform InteractPoint => _interactPoint;
    
    public override bool IsAvailable => Disabled == false && CurrentPrisoner == null && _upcomingPrisoner == null && gameObject.activeInHierarchy;
    public override bool Abandoned { get; protected set; }
    public bool Disabled { get; set; } = false;
    
    private float GetInteractTime()
    {
        if (_random == false)
            return _interactTime;
        return Random.Range(_interactTimeRange.Min(), _interactTimeRange.Max());
    }
    
    public override void ReservePlace(Prisoner prisoner)
    {
        _upcomingPrisoner = prisoner;
        if (CurrentPrisoner == prisoner)
            CurrentPrisoner = null;
    }

    protected override bool CanUsePlace(Prisoner prisoner)
    {
        return prisoner == _upcomingPrisoner;
    } 
        
    
    protected override void UsePlace(Prisoner prisoner)
    {
        if(CurrentPrisoner != null)
            return;
        
        Abandoned = false;
        CurrentPrisoner = prisoner;
        if(prisoner.Enabled)
            prisoner.Animator.SetTrigger(_startAnimationTrigger);
        Used?.Invoke(prisoner);
        
        if(_limitedTime)
            _kickDelay = _timer.ExecuteWithDelay(()=>Kick(prisoner), GetInteractTime(), TimeScale.Scaled);

        if (_disableAgent) prisoner.Agent.enabled = false;
        if(_move) prisoner.Human.transform.DOMove(_actionPoint.position, _moveTime);
        if(_rotate)  prisoner.Human.transform.DORotate(_actionPoint.rotation.eulerAngles, _moveTime);
    }

    
    public override void Kick(Prisoner prisoner)
    {
        if (CurrentPrisoner == null)
        {
            _upcomingPrisoner = null;
            return;
        }
        
        if(CurrentPrisoner != prisoner)
            return;

        if (_kickDelay != null)
        {
            _kickDelay.Kill();
            _kickDelay = null;
        }

        if (Abandoned == false)
        {
            Abandoned = true;
            _previousPrisoner = CurrentPrisoner;
        
            _previousPrisoner.Animator.SetTrigger(_stopAnimationTrigger);
            _previousPrisoner.Agent.enabled = true;
            _previousPrisoner.Human.transform.position = _exitPoint.position;
            _previousPrisoner.Human.transform.rotation = _exitPoint.rotation;
        }
        
        
        CurrentPrisoner = null;
        _upcomingPrisoner = null;
        Kicked?.Invoke(_previousPrisoner);
    }




}
