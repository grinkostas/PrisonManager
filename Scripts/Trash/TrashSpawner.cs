using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private TrashFabric _trashFabric;
    [SerializeField] private int _maxTrashCount;

    public List<Trash> SpawnedTrash { get; private set; } = new List<Trash>();
    public int Count => SpawnedTrash.Count;

    public bool CanSpawn()
    {
        return SpawnedTrash.Count < _maxTrashCount;
    }
    
    public void Spawn()
    {
        if(CanSpawn() == false)
            return;
        var spawnedTrash = _trashFabric.Get();
        spawnedTrash.Cleaned += OnTrashCleaned;
        SpawnedTrash.Add(spawnedTrash);
    }

    private void OnTrashCleaned(Trash trash)
    {
        SpawnedTrash.Remove(trash);
        trash.Cleaned -= OnTrashCleaned;
    }
}
