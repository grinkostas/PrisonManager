using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixCore.Game.API;
using StaserSDK.Utilities;
using Zenject;

public class ZoneBounce : MonoBehaviour
{
    [SerializeField] private BuyFx _buyFx;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _iterationScale;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _zoomOutSpeed;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private AudioSource _billThrowAudio;
    [SerializeField] private AudioSource _billHitAudio;

    [Inject] private Timer _timer;
    [Inject] private IHapticService _hapticService;

    private float _additionalScale = 0.0f;

    private void OnEnable()
    {
        _buyFx.SpawnedNode += OnSpawnNode;
    }

    private void OnDisable()
    {
        _buyFx.SpawnedNode -= OnSpawnNode;
    }

    private void OnSpawnNode(GameObject node)
    {
        if(_billThrowAudio != null)
            _billThrowAudio.Play();
        _timer.ExecuteWithDelay(() => OnMoneyBillHitTheSpot(node), _spawnDelay);
    }

    private void OnMoneyBillHitTheSpot(GameObject node)
    {
        _additionalScale += _iterationScale;
        _additionalScale = Mathf.Max(_additionalScale, _maxScale);
        _hapticService.Selection();
        _targetTransform.localScale = Vector3.one * (1 + _additionalScale);
        if(_billHitAudio != null)
            _billHitAudio.Play();
    }

    private void Update()
    {
        _targetTransform.localScale = Vector3.Lerp(_targetTransform.localScale, Vector3.one, Time.deltaTime * _zoomOutSpeed);
    }
}
