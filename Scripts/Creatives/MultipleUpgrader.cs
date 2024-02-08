using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class MultipleUpgrader : MonoBehaviour
{
    [SerializeField] private List<Upgrader> _upgraders;
    [SerializeField] private int _startUpgrades = 0;

    private void Start()
    {
        for (int i = 0; i < _startUpgrades; i++)
        {
            foreach (var upgrader in _upgraders)
            {
                upgrader.Upgrade();
            }
        }
    }

    [Button("Upgrade")]
    private void Upgrade()
    {
        foreach (var upgrader in _upgraders)
        {
            upgrader.Upgrade();
        }
    }
}
