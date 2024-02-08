using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;

public class Criminal : MonoBehaviour
{
    [SerializeField] private Human _human;
    [SerializeField] private StackSize _stackSize;
    public Animator Animator => _human.Animator;
    public StackSize Size => _stackSize;
    public int Level => _human.Level;
    public Human Human => _human;

    public static UnityAction<Human> Detected;
    public static Human LastDetected { get; private set; }

    private void OnEnable(){}
    private void OnDisable(){}

    public void Detect()
    {
        LastDetected = _human;
        Detected?.Invoke(_human);
    }
    





}

