using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Zenject;

public class Trash : MonoBehaviour
{
    [SerializeField] private List<TrashFragment> _trashFragments;
    [SerializeField] private View _view;
    [SerializeField] private float _destroyDelay;
    [SerializeField] private StackItem _trashItem;

    [Inject] private HandStack _handStack;
    
    private List<GameObject> _trashElements = new List<GameObject>();

    public float LifeTime { get; private set; } = 0.0f;
    
    public UnityAction<Trash> Cleaned;
    
    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        LifeTime += Time.deltaTime;
    }

    private void Spawn()
    {
        foreach (var fragment in _trashFragments)
        {
            _trashElements.Add(fragment.Spawn());
        }
    }


    public void Clear()
    {
        Cleaned?.Invoke(this);
        _view.Hide();
        StackItem stackItem = Instantiate(_trashItem, transform.position, Quaternion.identity);
        _handStack.Add(stackItem);
        Destroy(gameObject, _destroyDelay);
    }
}
