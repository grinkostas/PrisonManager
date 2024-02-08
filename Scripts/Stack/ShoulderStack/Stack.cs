using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NepixCore.Game.API;
using StaserSDK.Upgrades;
using Unity.VisualScripting;
using UnityEngine.Events;
using Zenject;

public class Stack : MonoBehaviour, IStack
{
    [SerializeField] private UpgradeValue _stackMaxSize;
    [SerializeField] private Transform _startStackPoint;
    [SerializeField] private int _finalMaxSize;
    
    [Inject] private IHapticService _hapticService;
    
    private List<CriminalStackData> _criminals = new List<CriminalStackData>();

    public List<CriminalStackData> Criminals => _criminals;

    private Vector3 _currentDelta = Vector3.zero;
    
    public UnityAction<int> CountChanged { get; set; }
    public UnityAction<CriminalStackData> AddedItem { get; set; }
    public UnityAction<Criminal, Transform> TookItem;

    
    public int ItemsCount => _criminals.Count;
    public UpgradeValue MaxSize => _stackMaxSize;
    public int FinalMaxSize => _finalMaxSize;
    public bool Full => ItemsCount >= _stackMaxSize.ValueInt;
    public Transform StackPoint => _startStackPoint;
    
    public class CriminalStackData
    {
        public Criminal Criminal { get; private set; }
        public Vector3 Delta { get; set; }

        public CriminalStackData(Criminal criminal, Vector3 delta)
        {
            Criminal = criminal;
            Delta = delta;
        }
    }

    public void Add(Criminal criminal)
    {
        if(ItemsCount >= _stackMaxSize.ValueInt) return;
        _currentDelta += new Vector3(0, criminal.Size.Size.y + criminal.Size.Center.y, 0);
        var newCriminalData = new CriminalStackData(criminal, _currentDelta);
        _criminals.Add(newCriminalData);
        CountChanged?.Invoke(_criminals.Count);
        AddedItem?.Invoke(newCriminalData);
        _hapticService.Selection();
    }

    public bool TryTakeAndPlace(int level, Transform destination, out Human human)
    {
        return TryTakeAndPlace((x => x.Criminal.Level == level), destination, out human);
    }

    public bool TryTakeAndPlace(Human humanPrefab, Transform destination, out Human human)
    {
        return TryTakeAndPlace((x => x.Criminal.Human.Id == humanPrefab.Id), destination, out human);
    }

    public bool TryTakeAndPlace(Transform destination)
    {
        return TryTakeAndPlace(x => true, destination, out Human human, true);
    }

    private bool TryTakeAndPlace(Predicate<CriminalStackData> predicate, Transform destination, out Human human, bool changeParent = false)
    {
        human = null;
        if (ItemsCount == 0) return false;
        var criminalDataToTake = _criminals.FindLast(predicate);
        if (criminalDataToTake == null) return false;
        human = criminalDataToTake.Criminal.Human;
        human.transform.SetParent(destination);
        Remove(criminalDataToTake);
        TookItem?.Invoke(criminalDataToTake.Criminal, destination);
        _hapticService.Selection();
        Rebuild();
        return true;
    }

    public bool TryTake(out Criminal criminal)
    {
        criminal = null;
        if (_criminals.Count == 0)
            return false;
        var criminalData = _criminals[^1];
        Remove(criminalData);
        criminal = criminalData.Criminal;
        _hapticService.Selection();
        Rebuild();
        return true;
    }

    private void Remove(CriminalStackData criminal)
    {
        _criminals.Remove(criminal);
        CountChanged?.Invoke(_criminals.Count);
    }
    
    private void Rebuild()
    {
        var delta = Vector3.zero;
        foreach (var criminal in _criminals)
        {
            delta += new Vector3(0, criminal.Criminal.Size.Size.y, 0);
            var criminalTransform = criminal.Criminal.transform;
            criminalTransform.localPosition = delta;
        }
        _currentDelta = delta;
    }
    
    
}
