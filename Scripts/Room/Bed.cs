using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class Bed : ActionPlace
{
    [Header("Bed")]
    [SerializeField] private Room _room;
    [SerializeField] private Transform _populatePoint;
    [SerializeField] private float _leaveReloadTime = 1.5f;

    [Inject] private Timer _bedTimer;
    
    public bool IsActive => gameObject.activeSelf;
    public Room Room => _room;
    private Prisoner _currentPrisoner = null;

    public Prisoner Prisoner => _currentPrisoner;

    public Transform PopulatePoint => _populatePoint;

    public UnityAction OnPopulate { get; set; }
    public UnityAction<Prisoner> OnLeave { get; set; }

    private bool _isAvailable = true;
    
    public void Populate(Prisoner prisoner)
    {
        if(IsAvailable() == false) return;
        _currentPrisoner = prisoner;
        OnPopulate?.Invoke();
    }


    public void Leave()
    {
        var prisoner = _currentPrisoner;
        _currentPrisoner = null;
        CurrentPrisoner = null;
        _upcomingPrisoner = null;
        _isAvailable = false;
        _bedTimer.ExecuteWithDelay(() => _isAvailable = true, _leaveReloadTime);
        OnLeave?.Invoke(prisoner);
    }

    public bool IsAvailable() => 
        _currentPrisoner == null && gameObject.activeSelf && _isAvailable;
}
