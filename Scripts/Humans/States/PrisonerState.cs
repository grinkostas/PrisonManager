using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrisonerState : State
{
    [SerializeField] private Prisoner _prisoner;
    [SerializeField] private ScheduleFollower _scheduleFollower;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private float _outlineWidth = 1.5f;
    
    [SerializeField] private string _sadParameter = "Sad";

    private Outline Outline => _prisoner.Human.Variant.Outline;
    public override void OnEnter()
    {
        _prisoner.enabled = true;
        _scheduleFollower.enabled = true;
        _prisoner.Animator.SetBool(_sadParameter, true);
        _prisoner.Populated += OnPopulated;
    }

    private void OnPopulated(Bed bed)
    {
        Outline.OutlineColor = _outlineColor;
        Outline.OutlineWidth = _outlineWidth;
        Outline.OutlineMode = Outline.Mode.OutlineVisible;
        Outline.enabled = true;
    }
    
    public override void OnExit()
    {
        _prisoner.Populated -= OnPopulated;
        _scheduleFollower.enabled = false;
        _prisoner.enabled = false;
        _prisoner.Animator.SetBool(_sadParameter, false);
        
        Outline.OutlineMode = Outline.Mode.OutlineAll;
        Outline.enabled = false;
    }
}
