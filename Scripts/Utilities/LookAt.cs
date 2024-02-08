using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _lookAtTarget;

    private void Update()
    {
        if(_lookAtTarget.gameObject.activeSelf == false)
            return;
        transform.LookAt(_lookAtTarget);
    }
}
