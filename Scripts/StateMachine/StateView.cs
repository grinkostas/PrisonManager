using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateView : MonoBehaviour
{
    [SerializeField] private State _state;
    [SerializeField] private View _view;
    private void OnEnable()
    {
        _state.OnStateEnter += _view.Show;
        _state.OnStateExit += _view.Hide;
    }
    
    private void OnDisable()
    {
        _state.OnStateEnter -= _view.Show;
        _state.OnStateExit -= _view.Hide;
    }
}
