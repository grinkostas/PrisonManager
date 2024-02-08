using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;

public class InteractZoom : MonoBehaviour
{
    [SerializeField] private ZoneBase _zone;
    [SerializeField] private ZoomView _zoomView;

    private void OnEnable()
    {
        _zone.OnEnter += OnEnter;
        _zone.OnExit += OnExit;
    }

    private void OnDisable()
    {
        _zone.OnEnter -= OnEnter;
        _zone.OnExit -= OnExit;
    }

    private void OnEnter(InteractableCharacter character)
    {
        _zoomView.Show();
    }

    private void OnExit(InteractableCharacter character)
    {
        _zoomView.Hide();
    }
}
