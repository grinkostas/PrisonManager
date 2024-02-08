using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class ActionPlaceBase : MonoBehaviour
{
    public abstract ActionPlaceType Type { get; }
    public abstract Transform InteractPoint { get; }
    public abstract bool IsAvailable { get; }
    public abstract bool Abandoned { get; protected set; }
    public abstract float InteractTime { get; }
    
    public UnityAction<Prisoner> Used;
    public UnityAction<Prisoner> Kicked;

    public abstract void ReservePlace(Prisoner prisoner);

    public bool TryUsePlace(Prisoner prisoner)
    {
        if(CanUsePlace(prisoner) == false) return false;
        UsePlace(prisoner);
        return true;
    }

    protected abstract bool CanUsePlace(Prisoner prisoner);

    protected abstract void UsePlace(Prisoner prisoner);

    public abstract void Kick(Prisoner prisoner);
}
