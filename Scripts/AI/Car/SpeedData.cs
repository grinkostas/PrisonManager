using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedData
{
    public float TargetSpeed { get; set; }
    public object Sender { get; }

    public SpeedData(object sender, float targetSpeed = 0)
    {
        Sender = sender;
        TargetSpeed = targetSpeed;
    }
}
