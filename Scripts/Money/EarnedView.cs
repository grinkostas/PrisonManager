using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class EarnedView : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private TMP_Text _earnText;
    [SerializeField] private float _checkTime;

    [Inject] private Balance _balance;
    
    private float _currentEarned = 0.0f;
    
    private float _waitTime = 0.0f;

    private void OnEnable()
    {
        _balance.Earned += OnEarned;
    }

    private void OnDisable()
    {
        _balance.Earned -= OnEarned;
    }

    private void Update()
    {
        _waitTime -= Time.deltaTime;
        if(_waitTime <= 0.0f)
            Hide();
    }

    private void OnEarned(float amount)
    {
        _waitTime = _checkTime;
        _view.Show();
        _currentEarned += amount;
        _earnText.text = $"+{(int)Mathf.Ceil(_currentEarned)}";
    }

    private void Hide()
    {
        _view.Hide();
        _waitTime = 0.0f;
        _currentEarned = 0.0f;
    }

}
