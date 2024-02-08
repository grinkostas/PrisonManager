using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using UnityEngine.Audio;
using Zenject;

public class AudioLoading : MonoBehaviour
{
    [SerializeField] private string _masterVolumeParameter;
    [SerializeField] private float _audioEnableDelay;
    [SerializeField] private AudioMixerGroup _group;
    
    [Inject] private Timer _timer;
    
    private void Start()
    {
        _group.audioMixer.SetFloat(_masterVolumeParameter, -80f);
        _timer.ExecuteWithDelay(() =>
            _group.audioMixer.SetFloat(_masterVolumeParameter, 0), _audioEnableDelay);
    }
}
