using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prison : MonoBehaviour
{
    [SerializeField] private List<ActionZoneBase> _actionZones;

    public bool TryGetFreePlaceToAction(out ActionPlaceBase place)
    {
        place = null;
        var actionZone = _actionZones.Find(x => x.HasFreePlaces());
        if (actionZone == null) return false;
        place = actionZone.GetFreePlace();
        if (place == null) return false;

        return true;

    }
    
}
