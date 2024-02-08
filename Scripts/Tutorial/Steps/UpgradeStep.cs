using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeStep : TutorialStepBase
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _wrapperToEnable;
    [SerializeField] private View _viewToHide;
    protected override Transform Target => _target;

    public override void OnEnter()
    {
        base.OnEnter();
        _wrapperToEnable.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        _wrapperToEnable.SetActive(false);
        _viewToHide.Hide();
    }
}
