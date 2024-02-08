using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class ActionZoneBase : MoneyGenerator
{
    [SerializeField] private bool _pay = true;
    private void OnEnable()
    {
        foreach (var actionPlace in GetAllPlaces())
            actionPlace.Kicked += OnKicked;
    }

    private void OnDisable()
    {
        foreach (var actionPlace in GetAllPlaces())
            actionPlace.Kicked -= OnKicked;
    }

    public abstract List<ActionPlaceBase> GetAllPlaces();

    private void OnKicked(Prisoner prisoner)
    {
        if(_pay) 
            Generated?.Invoke();
    } 
    
    public bool HasFreePlaces() => GetAllPlaces().Any(x => x.IsAvailable);

    public ActionPlaceBase GetFreePlace() => GetAllPlaces().Find(x => x.IsAvailable);

    public bool TryGetFreePlace(out ActionPlaceBase place)
    {
        place = null;
        if (HasFreePlaces() == false) 
            return false;
        place = GetFreePlace();
        return true;
    }
}
