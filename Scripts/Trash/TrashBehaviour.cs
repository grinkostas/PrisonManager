using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrashBehaviour : MonoBehaviour
{
    [SerializeField] private List<TrashSpawner> _trashSpawners;

    public int TrashCount => _trashSpawners.Sum(x => x.Count);

}
