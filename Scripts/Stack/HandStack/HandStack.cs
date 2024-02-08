using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixCore.Game.API;
using UnityEngine.Events;
using Zenject;

public class HandStack : MonoBehaviour, IStack
{
    [SerializeField] private int _maxItems;
    
    
    [Inject] private IHapticService _hapticService;
    
    private List<StackItem> _stackItems = new List<StackItem>();
    public List<StackItem> Items => _stackItems;
    public UnityAction<int> CountChanged { get; set; }
    public int ItemsCount => _stackItems.Count;
    public int MaxSize => _maxItems;
    public UnityAction<Transform, Vector3> AddedItem;
    public UnityAction<Transform, Transform> TookItem;

    public void Add(StackItem stackItem)
    {
        Vector3 destination = GetDestination();
        _stackItems.Add(stackItem);
        CountChanged?.Invoke(ItemsCount);
        AddedItem?.Invoke(stackItem.transform, destination);
        _hapticService.Selection();
    }

    public bool TryTake(StackItemType stackItemType, out StackItem stackItem, Transform destination)
    {
        stackItem = _stackItems.FindLast(x => x.Type == stackItemType);
        if (stackItem == null)
            return false;
        _stackItems.Remove(stackItem);
        TookItem?.Invoke(stackItem.transform, destination);
        CountChanged?.Invoke(ItemsCount);
        return true;
    }

    private Vector3 GetDestination()
    {
        Vector3 localPosition = Vector3.zero;
        
        foreach (var stackItem in _stackItems)
        {
            localPosition.y += stackItem.StackSize.Size.y;
            localPosition += stackItem.StackSize.Center;
        }

        return localPosition;
    }

    
}
