using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceUpgradeZone : MonoBehaviour
{
    [SerializeField] private List<PlaceUpgradeModel> _models;
    [SerializeField] private PlayerZone _playerZone;

    private void OnEnable()
    {
        foreach (var model in _models)
        {
            model.Upgraded += Actualize;
        }
        Actualize();
    }

    private void OnDisable()
    {
        foreach (var model in _models)
        {
            model.Upgraded -= Actualize;
        }
    }
    
    private void Actualize()
    {
        foreach (var model in _models)
        {
            if(model.CanLevelUp)
                return;
        }
        _playerZone.gameObject.SetActive(false);
    }
}
