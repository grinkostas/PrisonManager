using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;

public class TrashCleaner : MonoBehaviour
{
    [SerializeField] private Trash _trash;
    [SerializeField] private ZoneBase _interactableZone;
    [SerializeField] private ParticleSystem _clearParticle;
    private void OnEnable()
    {
        _interactableZone.OnInteract += Clear;
    }
    
    private void OnDisable()
    {
        _interactableZone.OnInteract -= Clear;
    }

    private void Clear(InteractableCharacter character)
    {
        if(_clearParticle != null)
            _clearParticle.Play();
        _trash.Clear();
    }
    
}
