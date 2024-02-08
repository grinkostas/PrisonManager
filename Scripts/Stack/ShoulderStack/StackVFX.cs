using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using NaughtyAttributes;
using Zenject;

public class StackVFX : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [SerializeField] private float _particleDestroyDelay = 1.5f;
    [SerializeField] private MoveFx _fx;
    [Header("Take")]
    [SerializeField] private View _stackMaxView;
    [SerializeField] private Vector3 _addDelta;
    [SerializeField] private GameObject _poofParticle;
    [Header("Fall")] 
    [SerializeField] private ParticleSystem _landParticle;
    [SerializeField] private float _fallTime;
    [Header("Bounce")]
    [SerializeField] private float _bounceDelta;
    [SerializeField] private float _bounceTime;
    [Header("Triggers")]
    [SerializeField] private Animator _animatorToGetTriggers;
    [SerializeField, AnimatorParam(nameof(_animatorToGetTriggers))]
    private string _layOnShoulderTrigger;

    [SerializeField, AnimatorParam(nameof(_animatorToGetTriggers))]
    private string _floatingTrigger;
    
    [SerializeField, AnimatorParam(nameof(_animatorToGetTriggers))]
    private string _idleFromLayOnShoulderTrigger;

    [Inject] private Timer _timer;
    
    private void OnEnable()
    {
        _stack.AddedItem += OnAddItem;
        _stack.TookItem += OnTookItem;
    }
    
    private void OnDisable()
    {
        _stack.AddedItem -= OnAddItem;
        _stack.TookItem -= OnTookItem;
    }

    private void OnAddItem(Stack.CriminalStackData stackData)
    {
        stackData.Criminal.Animator.SetTrigger(_layOnShoulderTrigger);
        stackData.Criminal.enabled = false;

        var particle = Instantiate(_poofParticle, stackData.Criminal.transform.position, Quaternion.identity);
        Destroy(particle, _particleDestroyDelay);

        Transform criminalTransform = stackData.Criminal.transform;
        criminalTransform.SetParent(_stack.StackPoint);
        criminalTransform.transform.localRotation = Quaternion.identity;
        criminalTransform.localPosition = stackData.Delta + _addDelta;
        criminalTransform.DOLocalMove(stackData.Delta, _fallTime);
        if (_stack.Full)
        {
            _stackMaxView.Show();
        }
    }
    
    

    private void OnTookItem(Criminal criminal, Transform destination)
    {
        criminal.transform.SetParent(destination);
        _fx.Move(criminal.transform, destination, Vector3.zero);
        criminal.Animator.SetTrigger(_floatingTrigger);
        
        _timer.ExecuteWithDelay(Land, criminal, _fx.Duration);
    }

    private void Land(Criminal criminal)
    {
        criminal.Animator.SetTrigger(_idleFromLayOnShoulderTrigger);
        Transform targetTransform = criminal.Human.Variant.transform;

        Instantiate(_landParticle, targetTransform.position, Quaternion.identity);
        targetTransform.DOPunchScale(new Vector3(0,  -_bounceDelta, 0), _bounceTime, 2);
    }
    
}
