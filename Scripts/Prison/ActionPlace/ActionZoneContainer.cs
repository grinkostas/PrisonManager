using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionZoneContainer : ActionZoneBase
{
    [SerializeField] private List<ActionPlaceContainer> _actionPlaceContainers;


    private List<ActionPlaceBase> _places = new List<ActionPlaceBase>();
    private List<ActionPlaceBase> Places
    {
        get
        {
            if (_places.Count == 0)
                foreach (var actionPlaceContainer in _actionPlaceContainers)
                {
                    _places.AddRange(actionPlaceContainer.ActionPlaces);
                }

            return _places;
        }
    }

    public override List<ActionPlaceBase> GetAllPlaces() => Places;

}
