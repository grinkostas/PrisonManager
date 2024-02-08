using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ModelRandomer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects;

    private void Start()
    {
        var enableObject = _objects[Random.Range(0, _objects.Count)];
        foreach (var obj in _objects)
        {
            obj.SetActive(false);
        }
        enableObject.SetActive(true);
        
    }
}
