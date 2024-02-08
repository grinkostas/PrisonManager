using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private List<Upgradable> _upgradablePlaces;
    [SerializeField] private List<ParticleSystem> _upgradeParticles;
    [SerializeField] private int _startLevel;

    private int _level = 1;

    private void OnEnable()
    {
        _level = _startLevel;
        foreach (var upgradablePlace in _upgradablePlaces)
        {
            upgradablePlace.UpgradeToLevel(_level);
            upgradablePlace.Enabled += OnEnabled;
        }
    }

    private void OnEnabled(Upgradable upgradable)
    {
        upgradable.UpgradeToLevel(_level);
    }

    public void Upgrade(int level)
    {
        if (_level < level)
        {
            Upgrade();
            Upgrade(level);
        }
    }
    
    [Button("Upgrade")]
    public void Upgrade()
    {
        _level++;
        foreach (var upgradablePlace in _upgradablePlaces)
        {
            upgradablePlace.UpgradeToLevel(_level);
            if(upgradablePlace.gameObject.activeSelf == false)
                continue;
            
            foreach (var particle in _upgradeParticles)
            {
                Instantiate(particle, upgradablePlace.transform);
            }

        }
    }
}
