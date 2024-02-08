using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK;
using Zenject;

public class StackElasticity : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [Header("Position")]
    [SerializeField] private float _maxOffset;
    [SerializeField] private Vector3 _offsetAxis;
    [Header("Rotation")]
    [SerializeField] private float _maxRotation;
    [SerializeField] private Vector3 _rotationAxis;
    [Header("Curves")]
    [SerializeField] private AnimationCurve _deformationCurve;
    [SerializeField] private AnimationCurve _moveCurve;
    [Header("Return")]
    [SerializeField] private Ease _returnEase;
    [SerializeField] private float _returnDuration;
    [SerializeField] private float _takeReturnDuration = 0.25f;
    [Space]
    [SerializeField] private float _distanceToMaxEffect;
    [SerializeField] private float _duration;

    [Inject] private InputHandler _inputHandler;
    private List<Tweener> _currentTweeners = new List<Tweener>();
    
    private List<Transform> ItemsTransforms => _stack.Criminals.Select(x => x.Criminal.Human.Variant.transform).ToList();
    private Vector3 _startPosition;
    private Vector3 _crossedDistance = Vector3.zero;
    private float _wastedTime = 0.0f;


    private bool _started = false;
    private bool _stopped = true;

    private Vector3 _previousPosition;
    
    private void OnEnable()
    {
        //_inputHandler.OnStartMove.AddListener(OnStartMove);
        //_inputHandler.OnMove.AddListener(OnMove);
        //_inputHandler.OnStopMove.AddListener(OnStopMove);
        _stack.TookItem += OnTookItem;
    }
    
    private void Awake()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        _wastedTime += Time.deltaTime;
        Vector3 deltaVector = transform.position - _previousPosition;
        _previousPosition = transform.position;
        if (Mathf.Abs(deltaVector.x) + Mathf.Abs(deltaVector.z) < 0.01f)
        {
            OnStopMove();
            return;
        }

        if (_started == false)
        {
            OnStartMove();
            return;
        }
        
        OnMove(deltaVector);
        
        
    }

    private void OnTookItem(Criminal criminal, Transform transform)
    {
        Transform targetTransform = criminal.Human.Variant.transform;
        Tweener tweener = _currentTweeners.Find(x => (Transform)x.target == targetTransform);
        if(tweener != null)
            tweener.Kill();
        targetTransform.localPosition = Vector3.zero;
        targetTransform.localRotation = Quaternion.identity;
    }

    private void OnStartMove()
    {
        if(_started)
            return;
        
        _started = true;
        _stopped = false;
        KillTweeners();
        _startPosition = transform.position;
        _wastedTime = 0.0f;
    }

    public void OnStopMove()
    {
        if(_stopped)
            return;
        
        _crossedDistance = Vector3.zero;;
        _wastedTime = 0.0f;
        _started = false;
        _stopped = true;
        
        for (int i = 0; i < ItemsTransforms.Count; i++)
        {
            Vector3 moveDestination = _maxOffset * _offsetAxis * _deformationCurve.Evaluate(i / (float)_stack.FinalMaxSize) * -0.5f;
            Vector3 rotationDestination = _rotationAxis * _maxRotation * _deformationCurve.Evaluate(i / (float)_stack.FinalMaxSize) * -0.5f;
            
            _currentTweeners.Add(ItemsTransforms[i].DOLocalMove(moveDestination, _returnDuration).SetEase(_returnEase));
            _currentTweeners.Add(ItemsTransforms[i].DOLocalMove(Vector3.zero, _returnDuration).SetEase(_returnEase).SetDelay(_returnDuration));
            
            _currentTweeners.Add(ItemsTransforms[i].DOLocalRotate(rotationDestination, _returnDuration).SetEase(_returnEase));
            _currentTweeners.Add(ItemsTransforms[i].DOLocalRotate(Vector3.zero, _returnDuration).SetEase(_returnEase).SetDelay(_returnDuration));
        }
    }


    private void KillTweeners()
    {
        foreach (var tweener in _currentTweeners)
        {
            tweener.Kill();
        }
        _currentTweeners.Clear();
    }

    
    
    private void OnMove(Vector3 inputVector)
    {
        _stopped = false;

        _crossedDistance += inputVector;
        float distance = _crossedDistance.magnitude;
        float distanceCoefficient = Mathf.Clamp(distance / _distanceToMaxEffect, 0f, 1f);
        float timeCoefficient = _moveCurve.Evaluate(Mathf.Clamp(_wastedTime / _duration, 0f, 1f));
        Vector3 moveDestination = _maxOffset * _offsetAxis;
        Vector3 rotationDestination = _maxRotation * _rotationAxis; 
        
        for (int i = 0; i < ItemsTransforms.Count; i++)
        {
            float curveCoefficient = _deformationCurve.Evaluate(i / (float)_stack.FinalMaxSize);
            Vector3 localMoveDestination = moveDestination * curveCoefficient;
            Quaternion localRotateDestination = Quaternion.Euler(rotationDestination * curveCoefficient);
            ItemsTransforms[i].localPosition = Vector3.Lerp(ItemsTransforms[i].localPosition, localMoveDestination, timeCoefficient);
            ItemsTransforms[i].localRotation = Quaternion.Lerp(ItemsTransforms[i].localRotation, localRotateDestination, timeCoefficient);
        }


    }
}
