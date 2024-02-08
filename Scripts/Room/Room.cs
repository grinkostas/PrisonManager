using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine.Events;
using Zenject;

public class Room : MoneyGenerator
{
    [SerializeField] private List<Bed> _beds;
    [SerializeField] private ActionZone _roomActionZone;
    [SerializeField] private float _takeDelay;
    [SerializeField, HideIf(nameof(_specialRoom))] private int _roomLevel;
    [SerializeField] private bool _specialRoom;
    [SerializeField, ShowIf(nameof(_specialRoom))] private Human _humanToPopulatePrefab;
    
    public ActionZone ActionZone => _roomActionZone;
    public List<Bed> Beds => _beds;
    public List<Prisoner> Prisoners { get; private set; } = new List<Prisoner>();

    private float _timer = 0.0f;
    public int Level => _roomLevel;


    private void OnEnable()
    {
        foreach (var bed in _beds)
        {
            bed.OnLeave += Generate;
        }
    }
    private void OnDisable()
    {
        foreach (var bed in _beds)
        {
            bed.OnLeave -= Generate;
        }
    }

    private void Generate(Prisoner prisoner)
    {
        Prisoners.Remove(prisoner);
    }
    
    private void Update()
    {
        if(_timer > 0.0f)
            _timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Stack stack))
        {
            TakeFromStack(stack);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Stack stack))
        {
            TakeFromStack(stack);
        }
    }

    private void TakeFromStack(Stack stack)
    {
        if (_timer > 0.05f) return;

        var bed = _beds.Find(x => x.IsAvailable());
        if(bed == null) return;

        Human human;
        bool populate = false;

        if (_specialRoom)
            populate = stack.TryTakeAndPlace(_humanToPopulatePrefab, bed.PopulatePoint, out human);
        else
            populate = stack.TryTakeAndPlace(_roomLevel, bed.PopulatePoint, out human);

        if (populate)
        {
            Place(bed, human);
            Generated?.Invoke();   
        }
        
    }

    private void Place(Bed bed, Human human)
    {
        human.Prisoner.Populate(bed);
        Prisoners.Add(human.Prisoner);
        bed.Populate(human.Prisoner);
        _timer = _takeDelay;
    }

    public List<Prisoner> GetPrisoners()
    {
        return _beds.Where(x => x.Prisoner != null).Select(x => x.Prisoner).ToList();
    }

}
