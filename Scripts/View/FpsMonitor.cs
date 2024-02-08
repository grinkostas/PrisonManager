using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class FpsMonitor : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private float _updateTime;

    [Inject] private Updater _updater;

    private void Awake()
    {
        _updater.Add(this, OnUpdate, _updateTime);
    }

    private void OnUpdate()
    {
        if (Time.timeScale > 0)
        {
            _fpsText.text = Mathf.CeilToInt(1.0f / Time.deltaTime).ToString();
        }
    }
}
