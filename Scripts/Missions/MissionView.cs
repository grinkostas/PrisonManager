using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MissionView : MonoBehaviour
{
    [SerializeField] private TMP_Text _missionDescription;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _hintButton;

    private MissionState _mission;

    private void OnEnable()
    {
        _hintButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _hintButton.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if(_mission == null)
            return;
        if(_mission.HintCamera == null)
            return;
        _mission.HintCamera.gameObject.SetActive(true);
    }

    public void Init(MissionState mission)
    {
        if (_mission != null)
            _mission.ProgressChanged -= OnProgressChanged;

        _mission = mission;
        _missionDescription.text = _mission.Description;
        _icon.sprite = _mission.Icon;
        _progressSlider.value = 0.0f;
        _mission.ProgressChanged += OnProgressChanged;
    }

    private void OnProgressChanged(float progress)
    {
        _progressSlider.value = progress;
    }
}
