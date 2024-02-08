using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CarBrakesBase : MonoBehaviour
{
    [SerializeField] private bool _reverse;
    protected abstract float Speed { get; }
    protected float TargetSpeed = float.PositiveInfinity;
    
    protected List<SpeedData> Brakes { get; private set; } = new List<SpeedData>();
    private void FixedUpdate()
    {
        ActualizeSpeed();
    }

    public virtual void Brake(object sender)
    {
        SetTargetSpeed(sender, 0);
    }

    public virtual void StopBrake(object sender)
    {
        Brakes.RemoveAll(x => x.Sender == sender);
    }

    public virtual void SetTargetSpeed(object sender, float targetSpeed)
    {
        if (targetSpeed < TargetSpeed ||
            (targetSpeed > TargetSpeed && _reverse))
            TargetSpeed = targetSpeed;
        
        var speedData = Brakes.Find(x => x.Sender == sender);
        if (speedData != null)
        {
            speedData.TargetSpeed = targetSpeed;
            return;
        }

        Brakes.Add(new SpeedData(sender, targetSpeed));
    }

    protected abstract void ActualizeSpeed();

}
