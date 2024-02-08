using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class Door : MonoBehaviour
{
    [SerializeField] private Transform _doorModel;
    [SerializeField] private Vector3 _openOffset;
    [SerializeField] private float _openDuration;
    [SerializeField] private AudioSource _openSound;

    private List<object> _closeBlockers = new List<object>();

    private Tweener _currentTweener = null;
    
    private Vector3 _startPosition;
    private bool _opened = false;

    private void Start()
    {
        _startPosition = _doorModel.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DoorKey key))
        {
            _closeBlockers.Add(key);
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DoorKey key))
        {
            _closeBlockers.Remove(key);
            if(_closeBlockers.Count == 0)
                Close();
        }
    }

    private void Open()
    {
        if(_opened) return;
        if(_openSound != null && _openSound.enabled)
            _openSound.Play();
        if(_currentTweener != null)
            _currentTweener.Kill();
        _opened = true;
        
        _currentTweener = _doorModel.DOMove(_startPosition + _openOffset, _openDuration);
    }

    private void Close()
    {
        if(_opened == false) return;
        _opened = false;
        
        if(_openSound != null)
            _openSound.Play();
            
        if(_currentTweener != null)
            _currentTweener.Kill();
        
        _currentTweener = _doorModel.DOMove(_startPosition, _openDuration);
    }
    
   
    
}
