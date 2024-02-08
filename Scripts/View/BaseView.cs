using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseView : View
{
    [SerializeField] private GameObject _model;
    public override bool IsHidden => _model.activeSelf;
    public override void Show()
    {
        _model.SetActive(true);
    }

    public override void Hide()
    {
        _model.SetActive(false); 
    }
}
