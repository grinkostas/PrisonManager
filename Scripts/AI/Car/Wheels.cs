using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wheels : MonoBehaviour
{
    [SerializeField] private Wheel _leftFront;
    [SerializeField] private Wheel _leftBack;
    [SerializeField] private Wheel _rightFront;
    [SerializeField] private Wheel _rightBack;
    [Header("Angles")] 
    [SerializeField] private float _minSteeringAngle;
    [SerializeField] private float _maxSteeringAngle;
    

    public Wheel[] Left => new[] { _leftFront, _leftBack };
    public Wheel[] Right => new[] { _rightFront, _rightBack };
    public Wheel[] Front => new[] { _leftFront, _rightFront };
    public Wheel[] Back => new[] { _leftBack, _rightBack };
    public Wheel[] All => new[] { _leftFront, _leftBack, _rightFront, _rightBack };

    public float MaxSteeringAngle => _maxSteeringAngle;
    
    public void Rotate(float coefficient, bool ignoreMinAngle = false)
    {
        float angle = _maxSteeringAngle * coefficient;
        if (ignoreMinAngle == false && coefficient > 0)
            angle = Mathf.Clamp(angle, _minSteeringAngle, _maxSteeringAngle);
        
        foreach (var wheel in Front)
        {
            wheel.Collider.steerAngle = angle;
            //wheel.Model.rotation = Quaternion.Euler(Vector3.up * angle);
        }
        foreach (var wheel in Back)
        { 
            //wheel.Collider.steerAngle = angle * -1;
            //wheel.Model.rotation = Quaternion.Euler(Vector3.up * (angle * -1));
        }
    }

    public void ResetRotate() => Rotate(0, true);

}
