using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;
using Zenject;

public class RadarDisabler : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [SerializeField] private Radar _radar;

    private List<object> _disablers = new List<object>();

    public Radar Radar => _radar;
    
    private void OnEnable()
    {
        _stack.MaxSize.Model.Upgraded += OnUpgraded;
        _stack.CountChanged += OnCountChanged;
        Criminal.Detected += OnDetectCriminal;
    }

    private void OnDisable()
    {
        _stack.MaxSize.Model.Upgraded -= OnUpgraded;
        _stack.CountChanged -= OnCountChanged;
        Criminal.Detected -= OnDetectCriminal;
    }

    private void OnUpgraded()
    {
        Enable(this);
    }
    
    private void OnCountChanged(int count)
    {
        if(_stack.Full)
            Disable(this);
        else if(_radar.gameObject.activeSelf == false)
            Enable(this);
    }

    private void OnDetectCriminal(Human human)
    {
        if (_stack.ItemsCount + 1 >= _stack.MaxSize.ValueInt)
        {
            Disable(this);
        }
    }

    public void Enable(object sender)
    {
        _disablers.Remove(sender);
        if (_disablers.Count == 0)
        {
            _radar.Enable();
            _radar.gameObject.SetActive(true);
        }
    }
    
    public void Disable(object sender)
    {
        if(_disablers.Contains(sender) == false)
            _disablers.Add(sender);
        _radar.Disable();
        _radar.gameObject.SetActive(false);
    }
}
