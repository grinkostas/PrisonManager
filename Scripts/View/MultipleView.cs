using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultipleView : View
{
    [SerializeField] private List<View> _views;

    private bool _isHidden = true;
    public override bool IsHidden => _isHidden;
    
    public override void Show()
    {
        _isHidden = false;
        foreach (var view in _views)
        {
            view.Show();
        }
    }

    public override void Hide()
    {
        _isHidden = true;
        foreach (var view in _views)
        {
            view.Hide();
        }
    }
}
