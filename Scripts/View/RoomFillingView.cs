using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Zenject;

public class RoomFillingView : MonoBehaviour
{
    [SerializeField] private Room _room;
    [SerializeField] private View _view;
    [SerializeField] private TMP_Text _fillingText;
    [SerializeField] private string _separator = "/";
    [SerializeField] private List<BuyZone> _roomBuyZones;
    [SerializeField] private float _customUpdateTime = 0.2f;

    [Inject] private Updater _updater;
    private void Awake()
    {
        _updater.Add(this, Actualize, _customUpdateTime, false);
    }

    private void OnEnable()
    {
        foreach (var bed in _room.Beds)
        {
            bed.OnPopulate += Actualize;
            bed.OnLeave += Actualize;
        }

        foreach (var buyZone in _roomBuyZones)
        {
            buyZone.Bought += Actualize;
        }
    }

    private void Start()
    {
        Actualize();
    }

    private void OnDisable()
    {
        foreach (var bed in _room.Beds)
        {
            bed.OnPopulate -= Actualize;
            bed.OnLeave -= Actualize;
        }

        foreach (var buyZone in _roomBuyZones)
        {
            buyZone.Bought -= Actualize;
        }
    }

    private void Actualize(Prisoner prisoner) => Actualize();

    private void Actualize()
    {
        int totalCount = _room.Beds.Count(x => x.IsActive);
        int availableCount = _room.Beds.Count(x => x.IsAvailable());
        if (availableCount == 0)
        {
            _fillingText.text = "Max";
        }
        else
        {
            _fillingText.text = $"{totalCount - availableCount}{_separator}{totalCount}";
            _view.Show();
        }
    }
    
    
}
