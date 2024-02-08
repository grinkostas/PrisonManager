using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;
using Random = UnityEngine.Random;

public class MoneyNodeFx : MonoBehaviour
{
    [SerializeField] private MoneyNode _moneyNode;
    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private float _zoomOutDelay;
    [SerializeField] private float _zoomOutDuration;
    [SerializeField] private float _rotateAngleLimit;
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private AudioSource _audioSource;
    
    [Inject] private Player _player;
    [Inject] private Timer _timer;
    
    private void OnEnable()
    {
        _moneyNode.OnClaim += OnClaim;
    }

    private void OnDisable()
    {
        _moneyNode.OnClaim -= OnClaim;
    }

    private void OnClaim(float progress)
    {
        _moveFx.Move(_moneyNode.transform, _player.transform, Vector3.zero, false, progress);
        var rotation = Random.Range(0,_rotateAngleLimit) * _rotateAxis;
        _moneyNode.transform.DORotate(rotation, _moveFx.Duration);
        _timer.ExecuteWithDelay(Delete, _zoomOutDelay);
    }

    private void Delete()
    {
        if(_audioSource != null && _audioSource.enabled)
            _audioSource.Play();
        _moneyNode.transform.DOScale(Vector3.zero, _zoomOutDuration)
            .OnComplete(() => Destroy(_moneyNode.gameObject));
    }
}
