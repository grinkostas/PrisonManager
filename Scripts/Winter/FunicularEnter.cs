using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;

public class FunicularEnter : MonoBehaviour
{
    [SerializeField] private ZoneBase _zoneEnterPoint;
    [SerializeField] private Transform _destination;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Funicular _funicular;

    public Transform FunicularDestination => _destination;
    public Transform PlayerExitPoint => _exitPoint;

    private void OnEnable()
    {
        _zoneEnterPoint.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        _zoneEnterPoint.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter interactableCharacter)
    {
        _funicular.Move(this);
    }
}
