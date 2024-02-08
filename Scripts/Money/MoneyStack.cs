using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Zenject;
using Random = UnityEngine.Random;

public class MoneyStack : MonoBehaviour
{
    [SerializeField] private MoneyNode _moneyNodePrefab;
    [SerializeField] private float _moneyGiveDelay;

    [SerializeField] private Vector2Int _stackSize;
    [SerializeField] private Transform _startPoint;

    [SerializeField] private MoveFx _moveFx;
    [SerializeField] private Vector2 _spawnRotationRange;
    [SerializeField] private Vector3 _rotateAxis;

    [SerializeField] private AudioSource _audioSource;
    
    private float _currentAmount;
    private List<MoneyNode> _moneyModels = new List<MoneyNode>();
    private Vector3Int _currentIndexes = Vector3Int.zero;

    private bool _playerInside = false;

    [Inject] private DiContainer _diContainer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _playerInside = true;
            StartCoroutine(GiveMoney());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _playerInside = false;
            StopCoroutine(GiveMoney());
        }
    }

    private IEnumerator GiveMoney()
    {
        int startCount = _moneyModels.Count;
        while (_playerInside)
        {
            float takeProgress = (startCount - _moneyModels.Count) / (float)startCount;
            TakeLast(takeProgress);
            yield return new WaitForSeconds(_moneyGiveDelay); 
        }
        
    }

    private void TakeLast(float progress)
    {
        if (_moneyModels.Count == 0)
        {
            return;
        }

        DecreaseIndexes();
        var money = _moneyModels.Last();
        _moneyModels.Remove(money);
        if(_moneyModels.Count == 0)
            _currentIndexes = Vector3Int.zero;
        
        _moveFx.EndMove(money.transform);
        money.Claim(progress);
    }

    [Button("Add")]
    public void Add()
    {
        SpawnMoney(1, _startPoint);
    }

    public void Add(float amount, Transform source)
    {
        int modelsCount = (int)amount / (int)_moneyNodePrefab.Amount;
        if (modelsCount <= 0)
            return;
        if(_audioSource != null && _audioSource.enabled)
            _audioSource.Play();
        SpawnMoney(modelsCount, source);
    }

    private void SpawnMoney(int count, Transform source)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnStackOfMoney(source);
        }
    }
    
    
    private void SpawnStackOfMoney(Transform source)
    {
        var node = _diContainer.InstantiatePrefab(_moneyNodePrefab, _startPoint);
        float randomAngle = Random.Range(_spawnRotationRange.Min(), _spawnRotationRange.Max());
        Quaternion rotation = Quaternion.Euler(_rotateAxis * randomAngle);
        node.transform.rotation = rotation;
        var money = node.GetComponent<MoneyNode>();
        _moneyModels.Add(money);
        Vector3 stackPoint = GetStackPoint();
        node.transform.position = source.position;
        _moveFx.Move(node.transform, _startPoint, stackPoint);
        IncreaseIndexes();
    }

    private void IncreaseIndexes()
    {
        if (_currentIndexes.x + 1 < _stackSize.x )
        {
            _currentIndexes += Vector3Int.right;
        }
        else 
        {
            if (_currentIndexes.z + 1 < _stackSize.y )
            {
                _currentIndexes.z += 1;
            }
            else
            {
                _currentIndexes.z = 0;
                _currentIndexes.y += 1;
            }
            _currentIndexes.x = 0;
        }
    }

    private void DecreaseIndexes()
    {
        _currentAmount = Mathf.Clamp(_currentAmount - (int)_moneyNodePrefab.Amount, 0, int.MaxValue);
        if (_currentIndexes.z - 1 > 0)
        {
            _currentIndexes.z -= 1;
            return;
        }

        if (_currentIndexes.x - 1 > 0)
        {
            _currentIndexes.z = _stackSize.y - 1;
            _currentIndexes.x -= 1;
            return;
        }

        _currentIndexes.y = Mathf.Clamp(_currentIndexes.y -1, 0, Int32.MaxValue);
        _currentIndexes.x = _stackSize.x - 1;
        _currentIndexes.z = _stackSize.y - 1;

    }
    
    private Vector3 GetStackPoint() 
    {
        var size = _currentIndexes;
        Vector3 positionDelta = _moneyNodePrefab.Center + _moneyNodePrefab.Size;
        Vector3 delta = Vector3.Scale(positionDelta, size);
        return delta;

    }
}
