using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Crosswalk : MonoBehaviour, ICarMovementControl
{
    public UnityAction<ICarMovementControl> AvailableToMove { get; set; }
    public Transform Transform => transform;
    private bool _canMove = true;
    private List<Human> _humans = new List<Human>();

    private bool _started = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Human human))
        {
            Add(human);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Human human))
        {
            Remove(human);
        }
    }

    private void Add(Human human)
    {
        if(_humans.Contains(human))
            return;
        _humans.Add(human);
        if (_started == false)
        {
            _started = true;
            _canMove = false;
        }
    }

    private void Remove(Human human)
    {
        _humans.Remove(human);
        if(_humans.Count > 0)
            return;
        AvailableToMove?.Invoke(this);
        _canMove = true;
        _started = false;
    }

    public bool CanMove()
    {
        return _canMove;
    }

}
