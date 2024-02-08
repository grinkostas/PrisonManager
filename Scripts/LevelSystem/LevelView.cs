using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Inject] private LevelSystem _levelSystem;

    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform Rect => _rectTransform;
    private void OnEnable()
    {
        Actualize();
    }

    public void Actualize()
    {
        _levelText.text = _levelSystem.CurrentLevel.ToString();
    }
}
