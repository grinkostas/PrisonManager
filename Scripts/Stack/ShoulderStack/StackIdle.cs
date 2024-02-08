using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class StackIdle : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [Header("Curves")] 
    [SerializeField] private AnimationCurve _deformationCurve;
    [SerializeField] private Ease _ease;
    [Header("Move")]
    [SerializeField] private float _offsetRange;
    [SerializeField] private Vector3 _offsetAxis;
    [Header("Rotation")]
    [SerializeField] private float _rotationOffsetRange;
    [SerializeField] private Vector3 _rotationAxis;
    [Header("Durations")]
    [SerializeField] private float _bounceDuration;
    [SerializeField] private float _returnDuration;

    private List<Tweener> _tweeners = new List<Tweener>();
    private List<Coroutine> _coroutines = new List<Coroutine>();

    private int _coefficient = -1;
    private void OnEnable()
    {
        _stack.AddedItem += OnAddedItem;
        _stack.TookItem += OnTookItem;
    }

    private void OnTookItem(Criminal criminal, Transform transform)
    {
        Rebuild();
        GetTransform(criminal).localPosition = Vector3.zero;
        GetTransform(criminal).localRotation = Quaternion.identity;
    }
    
    private void OnAddedItem(Stack.CriminalStackData stackData)
    {
        Rebuild();
    }

    private void Start()
    {
        StartCoroutine(CoefficientChanger());
    }

    private IEnumerator CoefficientChanger()
    {
        while (true)
        {
            _coefficient *= -1;
            yield return new WaitForSeconds(_bounceDuration);
        }
    }
    
    private void Rebuild()
    {
        StopCoroutines();
        for (int i = 0; i < _stack.Criminals.Count; i++)
        {
            _coroutines.Add(StartCoroutine(Bouncing(_stack.Criminals[i], i)));
        }
    }

    private void StopCoroutines()
    {
        foreach (var coroutine in _coroutines)
        {
            StopCoroutine(coroutine);
        }

        foreach (var tweener in _tweeners)
        {
            tweener.Kill();
        }

        _tweeners.Clear();
        _coroutines.Clear();
    }

    private IEnumerator Bouncing(Stack.CriminalStackData stackData, int i)
    {
        Vector3 destination = _offsetAxis * (_offsetRange * _deformationCurve.Evaluate(i/(float)_stack.FinalMaxSize));
        Vector3 rotationDestination = _rotationAxis * (_rotationOffsetRange * _deformationCurve.Evaluate(i/(float)_stack.FinalMaxSize));
        while (true)
        {
            var moveTweener = GetTransform(stackData.Criminal).DOLocalMove(destination * _coefficient, _bounceDuration).SetEase(_ease);
            var rotateTweener = GetTransform(stackData.Criminal).DOLocalRotate(rotationDestination * _coefficient, _bounceDuration).SetEase(_ease);
            _tweeners.Add(moveTweener);
            _tweeners.Add(rotateTweener);
            yield return new WaitForSeconds(_bounceDuration);
        }
    }

    private Transform GetTransform(Criminal criminal) => criminal.Human.Variant.Model;

    private class BounceData
    {
        public Coroutine Routine { get; }
        public Transform Target { get; }

        public BounceData(Coroutine coroutine, Transform target)
        {
            Routine = coroutine;
            Target = target;
        }
    }



}
