using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK;
using StaserSDK.Upgrades;
using StaserSDK.Utilities;
using TMPro;
using UnityEngine.Events;
using Zenject;

public class PlayerUpgradeEffect : MonoBehaviour
{
    [SerializeField] private List<Upgrade> _upgradesToListen;
    [SerializeField] private ZoomView _view;
    [SerializeField] private CinemachineVirtualCamera _effectCamera;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioSource _upgradeSound;
    [SerializeField] private float _showTime;
    [SerializeField] private float _cameraShowTime;
    [Header("Bounce Zoom")]
    [SerializeField] private bool _bounceZoom;
    [SerializeField, ShowIf(nameof(_bounceZoom))]
    private Transform _bounceZoomTarget;
    [SerializeField, ShowIf(nameof(_bounceZoom))]
    private float _zoomMultiplier;
    [SerializeField, ShowIf(nameof(_bounceZoom))]
    private float _bounceDuration;
    
    [Inject] private UpgradesController _upgradesController;
    [Inject] private RadarDisabler _radarDisabler;
    [Inject] private Timer _timer;

    private Timer.TimerDelay _showDelay;
    private Timer.TimerDelay _hideDelay;
    
    
    private List<UpgradeModel> _models = new List<UpgradeModel>();

    private List<UpgradeModel> Models
    {
        get
        {
            if(_models.Count == 0)
                foreach(var upgrade in _upgradesToListen)
                    _models.Add(_upgradesController.GetModel(upgrade));
            return _models;
        }
    }

    private readonly List<UnityAction> _unsubscribes = new List<UnityAction>();
    
    private void OnEnable()
    {
        foreach (var model in Models)
        {
            UnityAction subscribe = () => { OnUpgraded(model); };
            model.Upgraded += subscribe;
            _unsubscribes.Add(() => { model.Upgraded -= subscribe; });
        }
    }

    private void OnDisable()
    {
        foreach (var unsubscribe in _unsubscribes)
        {
            unsubscribe();
        }
    }


    private void OnUpgraded(UpgradeModel model)
    {
        Bounce();
        _particle.Play();
        if(_upgradeSound != null)
            _upgradeSound.Play();
        _view.Show();
        _effectCamera.gameObject.SetActive(true);
        _radarDisabler.Disable(this);
        if(_showDelay != null)
            _showDelay.Kill();
        if(_hideDelay != null)
            _hideDelay.Kill();
        _showDelay = _timer.ExecuteWithDelay(EffectEnd, _showTime, TimeScale.Scaled);
        _hideDelay = _timer.ExecuteWithDelay(()=> _effectCamera.gameObject.SetActive(false), _cameraShowTime, TimeScale.Scaled);
    }

    private void Bounce()
    {
        if(_bounceZoom == false) 
            return;

        _bounceZoomTarget.DOScale(Vector3.one * _zoomMultiplier, _bounceDuration / 2).OnComplete(
            () => _bounceZoomTarget.DOScale(Vector3.one, _bounceDuration / 2));
    }
    
    private void EffectEnd()
    {
        _particle.Stop();
        _view.Hide();
        _radarDisabler.Enable(this);
    }

}
