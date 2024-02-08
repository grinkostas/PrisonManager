using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionZone : ActionZoneBase
{
    [SerializeField] private List<ActionPlaceBase> _actionPlaces;

    public List<ActionPlaceBase> Places => _actionPlaces;

    public override List<ActionPlaceBase> GetAllPlaces() => _actionPlaces;

}
