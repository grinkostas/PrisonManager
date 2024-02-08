using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.AI;
using UnityEngine.Events;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class Prisoner : MonoBehaviour
{
    [SerializeField] private Human _human;
    [SerializeField] private Collider _collider;
    [SerializeField] private Vector2 _prisonerTimeRange;
    
    private float _prisonerTime = float.PositiveInfinity;

    [Inject] private DayPhaseChanger _dayPhaseChanger;
    [Inject] private Timer _timer;
    
    private float _timeInPrison = 0.0f;
    public Bed _currentBed;

    public List<string> _debugActionLogs = new List<string>();
    public ActionPlaceBase _currentPlace = null;
    public ActionPlaceBase _previousActionPlace = null;
    public ActionPlaceBase _destination = null;
    
    public ActionPlaceBase PreviousAction => _previousActionPlace;
    public Transform LeftHand => _human.Variant.LeftHand;
    public Transform RightHand => _human.Variant.RightHand;
    public Animator Animator => _human.Animator;
    public NavMeshAgentProvider Agent => _human.AgentHandler.Agent;
    public Human Human => _human;
    public Bed Bed => _currentBed;
    
    public ActionPlaceBase Destination => _destination;
    public ActionPlaceBase CurrentPlace => _currentPlace;

    public float TimeInPrison => _timeInPrison;
    public bool Enabled => enabled;
    public UnityAction<Bed> Populated;
    public UnityAction OnRelease;
    public UnityAction Kicked;

    public bool Escaped { get; private set; } = false;

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }

    public void Escape()
    {
        Escaped = true;
        if(_currentBed != null)
            _currentBed.Leave();
    }
    

    private void Update()
    {
        if(Enabled == false) 
            return;
        _timeInPrison += Time.deltaTime;
    }

    public void Populate(Bed bed)
    {
        enabled = true;
        _currentBed = bed;
        _timeInPrison = 0.0f;
        _prisonerTime = Random.Range(_prisonerTimeRange.Min(), _prisonerTimeRange.Max());
        Populated?.Invoke(bed);
    }

    [Button("Next Action")]
    public void NextAction()
    {
        if(Enabled == false) return;
        if(_currentPlace == _currentBed && _destination == _currentBed)
            return;
        List<ActionPlaceBase> places = GetAvailablePlaces();
        IntentVisit(places[0]);
    }

    public void GoBed()
    {
        IntentVisit(_currentBed);
    }

    public void IntentVisit(ActionPlaceBase place)
    {
        if(place == null)
            return;
        _debugActionLogs.Add($"{_timeInPrison}| try visit {place.name}");
        if (Enabled == false)
        {
            _debugActionLogs.Add($"{_timeInPrison}| disabled");
            return;
        }

        
        if (place != _currentBed)
        {
            if ((_currentPlace == place || _destination == place) && place != _currentBed)
            {
                _debugActionLogs.Add($"{_timeInPrison}| Same Place");
                return;
            }

            if (place == null || place.IsAvailable == false)
            {
                NextAction();
                return;
            }
        }
        else if(_currentPlace == _currentBed)
        {
            
            return;
        }

        Visit(place);
    }



    private void Visit(ActionPlaceBase place)
    {
        _debugActionLogs.Add($"{_timeInPrison}|visit {place.name}");
        _currentPlace = place;
        _destination = place;
        place.ReservePlace(this);
        place.Kicked += OnKicked;

        Animator.SetTrigger("DynIdle");
        Agent.enabled = true;
        _human.AgentHandler.SetDestination(place.InteractPoint.position);
    }
    
    
    
    private List<ActionPlaceBase> GetAvailablePlaces()
    {
        List<ActionPlaceBase> places = new List<ActionPlaceBase>();

        if (_dayPhaseChanger.CurrentPhase.Phase == DayPhase.Day && _currentBed.Room.ActionZone.TryGetFreePlace(out ActionPlaceBase place2))
        { 
            places.Add(place2);
        }

        places.Add(_currentBed);
        if(places.Count > 0)
            places.RemoveAll(
                x => x.Type != ActionPlaceType.Ignore && x.Type == (_previousActionPlace == null ? ActionPlaceType.Ignore : _previousActionPlace.Type));
        
        return places;
    }
    public void ForceKick(ActionPlaceBase place)
    {
        if(place == _currentPlace)
            return;
        
        if(_currentPlace == null)
            return;
        
        if(place.Type == _currentPlace.Type)
            return;
        
        if (_currentPlace != null)
        {
            _debugActionLogs.Add($"{_timeInPrison}| force kicked from {place.name}");
            
            Animator.SetTrigger("DynIdle");
            var placeToKick = _currentPlace;
            placeToKick.Kick(this); 
            _currentPlace = null;
        }
    }

    public void ForceKickToPlaceBed()
    {
        if(_currentPlace == null)
            return;
        if(_currentPlace == _currentBed)
            return;
        
        _debugActionLogs.Add($"{_timeInPrison}| kicked to place bed {_currentBed.name}");
        Animator.SetTrigger("DynIdle");
        _currentPlace.Kick(this);
        _currentPlace = null;
    }
    
    private void OnKicked(Prisoner current)
    {
        var currentPlace = _currentPlace;

        if (_currentPlace != null)
        {
            _debugActionLogs.Add($"{_timeInPrison}| kicked from {currentPlace.name}");
            _currentPlace.Kicked -= OnKicked;
            _previousActionPlace = _currentPlace;
            _currentPlace = null; 
            Animator.SetTrigger("DynIdle");
        }

        if (_timeInPrison >= _prisonerTime)
            Release();
        
        if(currentPlace != null)
            Kicked?.Invoke();
    }

    private void Release()
    {
        if(_currentBed == null)
            return;
        
        if (_previousActionPlace != null)
        {
            _previousActionPlace.Kick(this);
        }
        _debugActionLogs.Add($"{_timeInPrison}| leaved");
        _currentBed.Leave();
        OnRelease?.Invoke();
        _currentBed = null;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(Enabled == false) return;
        if (other.TryGetComponent(out ActionPlace place))
        {
            if (place.TryUsePlace(this))
            {
                _destination = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_currentPlace == null) return;
        if(Enabled == false) return;
        
        if (other.TryGetComponent(out ActionPlaceBase place))
        {
            if (place.TryUsePlace(this))
            {
                _destination = null;
            }
        }
    }
}
