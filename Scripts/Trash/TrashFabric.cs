using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class TrashFabric : MonoBehaviour
{
    [SerializeField] private Trash _trashPrefab;
    [SerializeField] private List<Transform> _trashPoints;

    [Inject] private DiContainer _container;
    
    private List<Transform> _usedPoints = new List<Transform>();

    public Trash Get()
    {
        List<Transform> availablePoints = _trashPoints.Except(_usedPoints).ToList();
        if (availablePoints.Count == 0)
        {
            availablePoints = _trashPoints;
            _usedPoints.Clear();
        }

        var randomPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        _usedPoints.Add(randomPoint);
        GameObject trashItemObject = _container.InstantiatePrefab(_trashPrefab, randomPoint);
        return trashItemObject.GetComponent<Trash>();
    }

}
