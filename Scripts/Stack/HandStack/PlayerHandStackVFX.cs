using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Zenject;
using StaserSDK.Utilities;


public class PlayerHandStackVFX : MonoBehaviour
{
    [SerializeField] private HandStack _handStack;
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private Transform _parentPoint;
    [SerializeField] private bool _updateCheck;
   
    [Header("Zoom")] 
    
    [SerializeField] private bool _dropZoomOut;

    [SerializeField, ShowIf(nameof(_dropZoomOut))]
    private float _zoomDuration;

    [SerializeField, ShowIf(nameof(_dropZoomOut))]
    private float _takeZoomDelay;


    
    private void OnEnable()
    {
        _handStack.AddedItem += OnAddedItem;
        _handStack.TookItem += TookItem;
    }
    
    private void OnDisable()
    {
        _handStack.AddedItem -= OnAddedItem;
        _handStack.TookItem -= TookItem;
    }

    private void Update()
    {
        if(_updateCheck == false)
            return;
        
    }

    private void OnAddedItem(Transform target, Vector3 destination)
    {
        _moveFx.Move(target, _parentPoint, destination, true, localMove:true);
        target.localScale = Vector3.zero;
        target.DOScale(Vector3.one, _zoomDuration);
    }

    private void TookItem(Transform target, Transform destination)
    {
        target.transform.SetParent(destination);
        target.DOScale(Vector3.zero, _zoomDuration).SetDelay(_takeZoomDelay);
        _moveFx.Move(target, destination);

    }
}
