using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSpawnCondition : TrashSpawnCondition
{
    [SerializeField] private Room _room;
    [SerializeField] private int _minPrisonersToStartSpawn = 1;
    [SerializeField] private float _timeDecreaseForPrisoner;
    protected override bool SpawnCondition()
    {
        return _minPrisonersToStartSpawn <= _room.Prisoners.Count;
    }

    protected override float GetAdditionalSpawnTime()
    {
        return -1 * _room.Prisoners.Count * _timeDecreaseForPrisoner;
    }
}
