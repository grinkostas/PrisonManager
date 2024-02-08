using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Transform _pointerRotateModel;
    [SerializeField] private Vector3 _lookAxis;
    [SerializeField] private float _minShowDistance;
    [SerializeField] private Transform _pointerUser;

    private List<PointerTarget> _targets = new List<PointerTarget>();
    
    private void Update()
    {
        if (_targets.Count == 0)
        {
            _pointerRotateModel.gameObject.SetActive(false);
            return;
        }

        Transform target = GetNearestTarget();
        _pointerRotateModel.LookAt(target);
        _pointerRotateModel.rotation = Quaternion.Euler(Vector3.Scale(_pointerRotateModel.rotation.eulerAngles, _lookAxis));
        _pointerRotateModel.gameObject.SetActive(GetDistance(target) >= _minShowDistance);
    }

    private Transform GetNearestTarget()
    {
        int maxPriority = _targets.Max(x => x.Priority);
        float minDistance = _targets.Where(x=> x.Priority == maxPriority).Min(x => GetDistance(x.Target));
        return _targets.Find(x => Math.Abs(GetDistance(x.Target) - minDistance) < 0.1f && x.Priority == maxPriority).Target;
    }

    private float GetDistance(Transform target)
    {
        if (target == null)
            return float.MaxValue;
        return Vector3.Distance(_pointerUser.position, target.position);
    }
    
    public void SetDestination(Transform destination, int priority = 0)
    {
        if(_targets.Count(x => x.Target == destination) > 0)
            return;
        _targets.Add(new PointerTarget(destination, priority));
    }

    public void ReceiveDestination(Transform destination)
    {
        if(_targets.Count(x => x.Target == destination) == 0)
            return;

        _targets.Remove(_targets.Find(x => x.Target == destination));
    }

    private class PointerTarget
    {
        public Transform Target { get; }
        public int Priority { get; }

        public PointerTarget(Transform target, int priority)
        {
            Target = target;
            Priority = priority;
        }
    }

}
