using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine.Events;
using Zenject;

public class MoneyNode : MonoBehaviour
{
    [SerializeField] private StackSize _stackSize;
    [SerializeField] private float _amount;

    [Inject] private Balance _balance;
    public float Amount => _amount;
    public Vector3 Center => _stackSize.Center;
    public Vector3 Size => _stackSize.Size;
    public UnityAction<float> OnClaim;

    public void Claim(float progress)
    {
        _balance.Earn(_amount);
        OnClaim?.Invoke(progress);
    }
}
