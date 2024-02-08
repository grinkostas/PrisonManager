using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TakeTrashState : TutorialStepBase
{
    [SerializeField] private TrashSpawner _trashSpawner;

    protected override Transform Target
    {
        get
        {
            if (_trashSpawner.Count == 0)
            {
                _trashSpawner.Spawn();
            }

            return _trashSpawner.SpawnedTrash.Last().transform;
        }
    }
}
