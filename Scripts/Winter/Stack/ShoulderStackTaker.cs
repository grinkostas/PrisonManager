using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Interactable;
using Zenject;

public class ShoulderStackTaker : MonoBehaviour
{
    [SerializeField] private ZoneBase _takeZone;
    [SerializeField] private Stack _shoulderStack;
    [Inject] private Stack _stack;

    private void OnEnable()
    {
        _takeZone.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        _takeZone.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter interactableCharacter)
    {
        if (_stack.TryTake(out Criminal criminal))
        {
            _shoulderStack.Add(criminal);
        }
    }
}
