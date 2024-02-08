using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class StackItem : MonoBehaviour
{
    [SerializeField] private StackItemType _stackItemType;
    [SerializeField] private StackSize _stackSize;
    [SerializeField] private Transform _wrapper;
    public StackSize StackSize => _stackSize;
    public StackItemType Type => _stackItemType;
    public Transform Wrapper => _wrapper;
}
