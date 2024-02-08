using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionPlaceContainer : MonoBehaviour
{
    [SerializeField] private List<ActionPlaceBase> _actionPlaces;

    public List<ActionPlaceBase> ActionPlaces => _actionPlaces;
}
