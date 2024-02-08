using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;

public class InteractZoneTransition : StateTransition
{
    [SerializeField] private ZoneBase _zoneBase;
    
    protected override void OnTransitionEnable()
    {
        _zoneBase.OnInteract += OnInteract;
    }

    protected override void OnTransitionDisable()
    {
        _zoneBase.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter character)
    {
        Transit();
    }
}
