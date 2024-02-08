using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionZoneMoneyReceiver : MoneyReceiver
{
    [SerializeField] private ActionZone _actionZone;

    private void OnEnable()
    {
        foreach (var actionPlace in _actionZone.Places)
        {
            actionPlace.Kicked += OnPlaceUsed;
        }
    }

    private void OnDisable()
    {
        foreach (var actionPlace in _actionZone.Places)
        {
            actionPlace.Kicked -= OnPlaceUsed;
        }
    }

    private void OnPlaceUsed(Prisoner prisoner)
    {
        Receive();
    }
}
