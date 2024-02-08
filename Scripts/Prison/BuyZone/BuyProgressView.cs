using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BuyProgressView : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private TMP_Text _priceText;


    private void OnEnable()
    {
        _buyZone.BuyProgressChanged += OnBuyProgressChanged;
    }

    private void OnDisable()
    {
        _buyZone.BuyProgressChanged -= OnBuyProgressChanged;
    }

    private void Start()
    {
        _priceText.text = ((int)_buyZone.Price).ToString();
    }

    private void OnBuyProgressChanged(float progress)
    {
        _priceText.text = (Mathf.Round(Mathf.Clamp(_buyZone.Price - progress, 0, _buyZone.Price))).ToString();
    }
}
