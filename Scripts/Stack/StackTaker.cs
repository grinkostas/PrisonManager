using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NepixCore.Game.API;
using StaserSDK.Interactable;
using UnityEngine.Events;
using Zenject;

public class StackTaker : MonoBehaviour
{
    [SerializeField] private StackItemType _stackItemType;
    [SerializeField] private ZoneBase _zoneBase;
    [SerializeField] private bool _destroyOnTake;
    [SerializeField, ShowIf(nameof(_destroyOnTake))] private float _destroyDelay;
    
    [Inject] private HandStack _handStack;
    [Inject] private IHapticService _hapticService;
    
    public UnityAction<StackItem> OnTake;

    private void OnEnable()
    {
        _zoneBase.OnInteract += OnInteract;
    }
    
    private void OnDisable()
    {
        _zoneBase.OnInteract -= OnInteract;
    }

    private void OnInteract(InteractableCharacter interactableCharacter)
    {
        var canTake = _handStack.TryTake(_stackItemType, out StackItem stackItem, transform);
        if (canTake == false)
            return;
        _hapticService.Selection();
        OnTake?.Invoke(stackItem);
        
        if(_destroyOnTake)
            Destroy(stackItem.gameObject, _destroyDelay);
    }
}
