using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

[RequireComponent(typeof(Collider))]
public class CatchZone : MonoBehaviour
{
    [Inject] private RadarDisabler _radarDisabler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _radarDisabler.Enable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _radarDisabler.Disable(this);
        }
    }
}
