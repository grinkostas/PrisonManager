using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public interface IStack
{
    public UnityAction<int> CountChanged { get; set; }
    public int ItemsCount { get; }
}
