using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private string _startText;
    [SerializeField] private string _additionalText;
    [SerializeField] private float _loopDuration;

    private void OnEnable()
    {
        _loadingText.text = _startText;
    }

    private void Start()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        float oneCharPeriod = _loopDuration / _additionalText.Length;
        int currentCharProgress = 0;
        while (enabled)
        {
            if (currentCharProgress > _additionalText.Length)
                currentCharProgress = 0;
            _loadingText.text = _startText + _additionalText.Substring(0, currentCharProgress);
            currentCharProgress++;
            yield return new WaitForSeconds(oneCharPeriod);
        }
    }
}
