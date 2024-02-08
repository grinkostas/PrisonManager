using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK;
using Unity.Mathematics;
using Zenject;

public class HandStackElasticity : MonoBehaviour
{
    [SerializeField] private HandStack _stack;
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
    
    private List<Transform> ItemsTransforms => _stack.Items.Select(x => x.Wrapper).ToList();
    private Vector3 _startPosition;
    private float _wastedTime = 0.0f;

    private void OnEnable()
    {
        _inputHandler.OnStartMove.AddListener(OnStartMove);
        _inputHandler.OnMove.AddListener(OnMove);
        _inputHandler.OnStopMove.AddListener(OnStopMove);
        _stack.TookItem += OnTookItem;
    }
    
    private void OnDisable()
    {
        _inputHandler.OnStartMove.RemoveListener(OnStartMove);
        _inputHandler.OnMove.RemoveListener(OnMove);
        _inputHandler.OnStopMove.RemoveListener(OnStopMove);
        _stack.TookItem -= OnTookItem;
    }
    
    private void Update()
    {
        _wastedTime += Time.deltaTime;
    }

    private void OnTookItem(Transform target, Transform destination)
    {
        Tweener tweener = _currentTweeners.Find(x => (Transform)x.target == target);
        if(tweener != null)
            tweener.Kill();
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
    }

    private void OnStartMove()
    {
        KillTweeners();
        _startPosition = transform.position;
        _wastedTime = 0.0f;
    }

    private void OnStopMove()
    {
        for (int i = 0; i < ItemsTransforms.Count; i++)
        {
            Transform targetTransform = ItemsTransforms[i];
            Vector3 moveDestination = _maxOffset * _offsetAxis * _deformationCurve.Evaluate(i / (float)_stack.MaxSize) * -0.5f;
            Vector3 rotationDestination = _rotationAxis * _maxRotation * _deformationCurve.Evaluate(i / (float)_stack.MaxSize) * -0.5f;
            
            _currentTweeners.Add(targetTransform.DOLocalMove(moveDestination, _returnDuration).SetEase(_returnEase));
            _currentTweeners.Add(targetTransform.DOLocalMove(Vector3.zero, _returnDuration).SetEase(_returnEase).SetDelay(_returnDuration));
            
            _currentTweeners.Add(targetTransform.DOLocalRotate(rotationDestination, _returnDuration).SetEase(_returnEase));
            _currentTweeners.Add(targetTransform.DOLocalRotate(Vector3.zero, _returnDuration).SetEase(_returnEase).SetDelay(_returnDuration));
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
        float distance = Vector3.Distance(transform.position, _startPosition);
        float distanceCoefficient = Mathf.Clamp(distance / _distanceToMaxEffect, 0f, 1f);
        float timeCoefficient = _moveCurve.Evaluate(Mathf.Clamp(_wastedTime / _duration, 0f, 1f));
        float transformationCoefficient = inputVector.magnitude * distanceCoefficient;
        Vector3 moveDestination = _maxOffset * _offsetAxis * transformationCoefficient;
        Vector3 rotationDestination = _maxRotation * _rotationAxis * transformationCoefficient; 
        
        
        for (int i = 0; i < ItemsTransforms.Count; i++)
        {
            Transform targetTransform = ItemsTransforms[i];
            float curveCoefficient = _deformationCurve.Evaluate(i / (float)_stack.MaxSize);
            Vector3 localMoveDestination = moveDestination * curveCoefficient;
            Vector3 localRotateDestination = rotationDestination * curveCoefficient;
            targetTransform.localPosition = Vector3.Lerp(ItemsTransforms[i].localPosition, localMoveDestination, timeCoefficient);
            targetTransform.localRotation = Quaternion.Lerp(ItemsTransforms[i].localRotation, quaternion.Euler(localRotateDestination), timeCoefficient);
        }


    }
}
