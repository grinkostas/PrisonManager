using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixCore.Game.API;
using StaserSDK.Utilities;
using Zenject;

public class UiView : View
{
    [SerializeField] private GameObject _objectToShow;
    [SerializeField] private bool _pause;

    [Inject] private PauseManager _pauseManager;
    
    [Inject] private IHapticService _hapticService;

    public override bool IsHidden => !_objectToShow.activeSelf;
    public override void Show()
    {
        if (_pause)
        {
            _pauseManager.Pause(this);
        }

        _hapticService.Selection();
        _objectToShow.SetActive(true);
    }

    public override void Hide()
    {
        if (_pause)
        {
            _pauseManager.Resume(this);
        }

        _objectToShow.SetActive(false);
    }
}
