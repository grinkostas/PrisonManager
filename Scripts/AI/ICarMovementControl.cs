
using UnityEngine;
using UnityEngine.Events;

public interface ICarMovementControl
{
    public bool CanMove();
    public UnityAction<ICarMovementControl> AvailableToMove { get; set; }
    public Transform Transform {get;}
}
