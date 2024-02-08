using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackGrabber : MonoBehaviour
{
    [SerializeField] private Stack _stack;
    [SerializeField] private View _maxView;

    private void OnEnable()
    {
        _stack.CountChanged += OnStackCountChanged;
    }

    private void OnDisable()
    {
        _stack.CountChanged -= OnStackCountChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Criminal criminal))
        {
            TryTakeCriminal(criminal);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Criminal criminal))
        {
            TryTakeCriminal(criminal);
        }
    }

    private void TryTakeCriminal(Criminal criminal)
    {
        if(criminal.enabled == false) return;
        if (_stack.Full)
            return;
        
        criminal.enabled = false;
        _stack.Add(criminal);
    }

    private void OnStackCountChanged(int count)
    {
        if(_stack.Full)
            _maxView.Show();
        else
            _maxView.Hide();
    }
}
