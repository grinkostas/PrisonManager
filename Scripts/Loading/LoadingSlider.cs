using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private float _startSliderValue;
    [SerializeField] private float _endSliderValue;
    [SerializeField] private float _slideDuration;
    [SerializeField] private Slider _slider;
}
