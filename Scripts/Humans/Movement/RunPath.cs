using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class RunPath : MonoBehaviour
{
    [SerializeField] private List<Transform> _pathPoints;
    [SerializeField] private float _minDistance = 0.5f;
    [Inject] private Player _player;
    private Vector3 PlayerPosition => _player.transform.position;
    public List<Transform> Points => _pathPoints;
    public float GetMinDistance(Transform senderTransform)
    {
        var points = _pathPoints.FindAll(x=> Vector3.Distance(PlayerPosition, x.position) > Vector3.Distance(senderTransform.position, x.position));
        if (points.Count == 0) 
            points = _pathPoints.FindAll(x=> Vector3.Distance(senderTransform.position, x.position) > _minDistance);

        return points.Min(x=> Vector3.Distance(senderTransform.position, x.position));
    }

    public Transform GetTargetPoint(Transform senderTransform, Transform ignorePoint = null)
    {
        var points = _pathPoints.FindAll(x=> Vector3.Distance(PlayerPosition, x.position) > Vector3.Distance(senderTransform.position, x.position));
        if (ignorePoint != null)
        {
            points.Remove(ignorePoint);
        }
        if (points.Count == 0) 
            points = _pathPoints.FindAll(x=> Vector3.Distance(senderTransform.position, x.position) > _minDistance);

        var sortedPoints = points.OrderBy(x=> Vector3.Distance(senderTransform.position, x.position));
        return points[Random.Range(0, points.Count)];
    }

    public int GetMinDistanceIndex(Transform senderTransform)
    {
        float distance = GetMinDistance(senderTransform);
        var runPathPoint = _pathPoints.Find(x =>
            (distance - Vector3.Distance(senderTransform.position, x.position)) < 0.1f);
        return _pathPoints.IndexOf(runPathPoint);
    }
}
