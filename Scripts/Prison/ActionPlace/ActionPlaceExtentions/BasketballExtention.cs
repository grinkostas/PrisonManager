using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class BasketballExtention : ActionPlaceExtention
{
    [SerializeField] private GameObject _ball;

    protected override void OnUsePlace(Prisoner prisoner)
    {
        _ball.SetActive(true);
    }

    protected override void OnKick(Prisoner prisoner)
    {
        _ball.SetActive(false);
    }
}
