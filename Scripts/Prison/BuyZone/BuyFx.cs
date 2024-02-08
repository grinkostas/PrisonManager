using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

public class BuyFx : MonoBehaviour
{
    [SerializeField] private Transform _payPoint;
    [Header("Delays")]
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _startZoomOutDelay;
    [SerializeField] private float _destroyDelay;
    [Header("Zoom")] 
    [SerializeField] private float _zoomOutZoom;
    [Header("Position")]
    [SerializeField] private Vector3 _positionSpawnDelta;
    [SerializeField] private Vector2 _deltaSpawnRange;
    [SerializeField] private Vector3 _deltaSpawnAxis;
    [Header("Rotation")]
    [SerializeField] private float _spawnAngleRange;
    [SerializeField] private Vector3 _rotateAxis;
    [Header("Dependencies")]
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private GameObject _moneyNodePrefab;
    [SerializeField] private MoveFx _moveFx;

    [Inject] private Timer _timer;
    [Inject] private Balance _balance;

    public UnityAction<GameObject> SpawnedNode;
    
    private List<GameObject> _spawnedNodes = new List<GameObject>();

    private List<Tweener> _zoomTweeners = new List<Tweener>();

    private void OnEnable()
    {
        _buyZone.Zone.OnInteract += SpawnMoney;
        _buyZone.Bought += OnBought;
    }

    private void OnDisable()
    {
        _buyZone.Zone.OnInteract -= SpawnMoney;
        
        _buyZone.Bought -= OnBought;
    }

    private void SpawnMoney(InteractableCharacter character)
    {
        if(_buyZone.Zone.IsCharacterInside == false || _buyZone.IsBought) return;
        if(_balance.Amount < 1) return;
        Vector3 rotate = Random.Range(0, _spawnAngleRange) * _rotateAxis;
        Vector3 localPositionDelta = Random.Range(_deltaSpawnRange.Min(), _deltaSpawnRange.Max()) * _deltaSpawnAxis;
        
        var moneyNode = Instantiate(_moneyNodePrefab, character.transform.position+_positionSpawnDelta, Quaternion.Euler(rotate));
        SpawnedNode?.Invoke(moneyNode);
        
        moneyNode.transform.localPosition += localPositionDelta;
        _moveFx.Move(moneyNode.transform, _payPoint, localPositionDelta, false, _buyZone.Spend/_buyZone.Price);
        _spawnedNodes.Add(moneyNode);
        moneyNode.transform.DORotate(rotate * 3, _moveFx.Duration);
        Tweener zoomOutTweener = moneyNode.transform.DOScale(Vector3.one * _zoomOutZoom, _moveFx.Duration - _startZoomOutDelay).SetDelay(_startZoomOutDelay);
        _zoomTweeners.Add(zoomOutTweener);
        _timer.ExecuteWithDelay(()=>_zoomTweeners.Remove(zoomOutTweener), _destroyDelay);
        Destroy(moneyNode, _destroyDelay);
        _timer.ExecuteWithDelay(SpawnMoney, character, _spawnDelay);
    }

    private void OnBought()
    {
        foreach (var zoomTweener in _zoomTweeners)
        {
            zoomTweener.Kill();
        }
        _zoomTweeners.Clear();
        foreach (var node in _spawnedNodes)
        {
            if(node == null)
                continue;
            node.transform.DOScale(Vector3.zero, _spawnDelay);
        }
        _spawnedNodes.Clear();
    }

    
    
    
}
